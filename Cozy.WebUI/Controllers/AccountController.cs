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
						message = "İstifadəçi adınız və ya şifrəniz yanlışdır"
					});
				}

				ViewBag.Message = "İstifadəçi adınız və ya şifrəniz yanlışdır";
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
						message = "İstifadəçi adınız və ya şifrəniz yanlışdır"
					});
				}

				ViewBag.Message = "İstifadəçi adınız və ya şifrəniz yanlışdır";
				goto end;
			}


			if (foundedUser.EmailConfirmed == false)
			{
				ViewBag.Message = "Zəhmət olmasa emailinizə gələn təsdiq linkindən hesabınızı doğrulayın";
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
						message = "İstifadəçi adınız və ya şifrəniz yanlışdır"
					});
				}

				ViewBag.Message = "İstifadəçi adınız və ya şifrəniz yanlışdır";
				goto end;
			}

			if (Request.IsAjaxRequest())
			{
				return Json(new
				{
					error = false,
					message = "Daxil oldunuz"
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

					var emailResponse = await emailService.SendEmailAsync(user.Email, "Registration for Cozy e-commerce Perfume website", $"Zəhmət olmasa abunəliyinizi <a href='{path}'>link</a> vasitəsilə təsdiq edin");

					if (emailResponse)
					{
						ViewBag.Message = "Qeydiyyat uğurla tamamlandı";
					}
					else
					{
						ViewBag.Message = " E-mail' göndərərkən xəta baş verdi, zəhmət olmasa yenidən cəhd edin";
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
				ViewBag.Message = "Xətalı token";
				goto end;
			}
			token = token.Replace(" ", "+");
			var result = await userManager.ConfirmEmailAsync(foundedUser, token);

			if (!result.Succeeded)
			{
				ViewBag.Message = "Xətalı token";
				goto end;
			}

			ViewBag.Message = "Hesabınız təsdiqləndi";
		end:
			return RedirectToAction(nameof(Signin));
		}

		[Route("/logout.html")]
		public async Task<IActionResult> Logout()
		{
			await signInManager.SignOutAsync();

			return RedirectToAction("Index", "home");
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