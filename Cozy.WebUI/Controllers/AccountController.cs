using Cozy.Domain.AppCode.Extensions;
using Cozy.Domain.AppCode.Services;
using Cozy.Domain.Models.Entities.Membership;
using Cozy.Domain.Models.FormModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cozy.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<CozyUser> signInManager;
        private readonly UserManager<CozyUser> userManager;
        private readonly EmailService emailService;
        private readonly RoleManager<CozyRole> roleManager;

        public AccountController(SignInManager<CozyUser> signInManager, UserManager<CozyUser> userManager, EmailService emailService, RoleManager<CozyRole> roleManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.emailService = emailService;
            this.roleManager = roleManager;
        }
		[AllowAnonymous]
		[Route("/signin.html")]
		public IActionResult Signin()
		{
			return View();
		}


        [AllowAnonymous]
        [HttpPost]
        [Route("/signin.html")]
        public async Task<IActionResult> Signin(LoginFormModel user)
        {
            CozyUser foundedUser = null;


            if (user.Email == null || user.Password == null)
            {
                if (Request.IsAjaxRequest())
                {
                    return Json(new
                    {
                        error = true,
                        message = "Username or password is not correct!"
                    });
                }

                ViewBag.Message = "Username or password is not correct!";
                goto end;
            }

            if (user.Email.IsEmail())
            {
                foundedUser = await userManager.FindByEmailAsync(user.Email);
            }
            else
            {
                foundedUser = await userManager.FindByNameAsync(user.Email);
            }

            if (foundedUser == null)
            {
                if (Request.IsAjaxRequest())
                {
                    return Json(new
                    {
                        error = true,
                        message = "Username or password is not correct!"
                    });
                }

                ViewBag.Message = "Username or password is not correct!";
                goto end;
            }


            if (foundedUser.EmailConfirmed == false)
            {
                ViewBag.Message = "Please verify your account with link sent to your email address";
                goto end;
            }

            var signInResult = await signInManager.PasswordSignInAsync(foundedUser, user.Password, true, true);



            if (!signInResult.Succeeded)
            {
                if (Request.IsAjaxRequest())
                {
                    return Json(new
                    {
                        error = true,
                        message = "Username or password is not correct!"
                    });
                }

                ViewBag.Message = "Username or password is not correct!";
                goto end;
            }

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    error = false,
                    message = "You logged in to your account"
                });
            }

            var callbackUrl = Request.Query["ReturnUrl"];

            if (!string.IsNullOrWhiteSpace(callbackUrl))
            {
                return Redirect(callbackUrl);
            }
            return RedirectToAction("Index", "Home");

        end:
            return View(user);
        }


        [AllowAnonymous]
		[Route("/register.html")]
		public IActionResult Register()
		{
			return View();
		}

		[AllowAnonymous]
		[HttpPost]
		[Route("/register.html")]
		public async Task<IActionResult> Register(RegisterFormModel model)
		{
			if (ModelState.IsValid)
			{
				var user = new CozyUser();

				user.Email = model.Email;
				user.UserName = model.Email;
				user.Name = model.Name;
				user.Surname = model.Surname;
				//user.EmailConfirmed = true;

				var result = await userManager.CreateAsync(user, model.Password);

				if (result.Succeeded)
				{
					var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
					string path = $"{Request.Scheme}://{Request.Host}/registration-confirm.html?email={user.Email}&token={token}";

					var emailResponse = await emailService.SendEmailAsync(user.Email, "Registration for Cozy e-commerce  website", $"Please confirm your subscription via <a href='{path}'>link</a>");

					if (emailResponse)
					{
						ViewBag.Message = "Registration completed successfully";
					}
					else
					{
						ViewBag.Message = "There was an error sending the email, please try again";
					}

					await userManager.AddToRoleAsync(user, "User");
					//await signInManager.SignInAsync(user, false);

					return RedirectToAction(nameof(Signin));
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(error.Code, error.Description);
                    ViewBag.Message = error.Description;
                }
			}

			return View(model);

		}

		[AllowAnonymous]
		[Route("registration-confirm.html")]
		public async Task<IActionResult> RegisterConfirm(string email, string token)
		{
			var foundedUser = await userManager.FindByEmailAsync(email);
			if (foundedUser == null)
			{
				ViewBag.Message = "Invalid token";
				goto end;
			}
			token = token.Replace(" ", "+");
			var result = await userManager.ConfirmEmailAsync(foundedUser, token);

			if (!result.Succeeded)
			{
				ViewBag.Message = "Invalid token";
				goto end;
			}

			ViewBag.Message = "Your account has been verified";
		end:
			return RedirectToAction(nameof(Signin));
		}

		[Route("/logout.html")]
		public async Task<IActionResult> Logout()
		{
			await signInManager.SignOutAsync();

			return RedirectToAction("Index", "home");
		}


        [AllowAnonymous]
        [Route("/notfoundpage")]
        public IActionResult NotFoundPage()
        {
            return View();
        }


        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordFormModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Email == null)
                {
                    ViewBag.Message = "Enter a valid email";

                    return View();
                }

                var user = await userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    ViewBag.Message = "Email is incorrect or has not been registered yet";

                    return View();
                }

                var isConfirmed = await userManager.IsEmailConfirmedAsync(user);

                if (!isConfirmed)
                {
                    ViewBag.Message = "Please confirm your email address first";

                    return View();
                }

                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, token = token, email = user.Email }, protocol: HttpContext.Request.Scheme);
                await emailService.SendEmailAsync(model.Email, "Reset Password",
                    $"<p style = 'font-size: 24px; font-weight: bold;'>Please reset your password by clicking <a href='{callbackUrl}'>here</a></p>");

                return RedirectToAction("ForgotPasswordConfirmation");
            }


            return View(model);
        }

        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult ResetPassword(string token = null, string email = null)
        {
            if (token == null || email == null)
            {
                return BadRequest("A code and email must be supplied for password reset.");
            }

            var model = new ResetPasswordFormModel { Token = token, Email = email };
            return View(model);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ViewBag.Message = "User not found";

                return View(model);
            }

            if (model.Password == null)
                goto end;

            if (model.Password != model.ConfirmPassword)
            {
                ViewBag.Message = "Passwords do not match!";

                return View(model);
            }

            var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                await signInManager.RefreshSignInAsync(user);

                await emailService.SendEmailAsync(model.Email, "Password Reset Successful",
                       "<p style = 'color: green; font-weight: bold; font-size: 24px;' > Your password has been changed successfully.</p>");

                return RedirectToAction("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

        end:
            ViewBag.Message = "Please enter a password";
            return View(model);
        }


        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        public async Task CreateRole()
        {
            if (!await roleManager.RoleExistsAsync("User"))
            {
                var userRole = new CozyRole { Name = "User" };

                await roleManager.CreateAsync(userRole);
            }

        }


    }
}