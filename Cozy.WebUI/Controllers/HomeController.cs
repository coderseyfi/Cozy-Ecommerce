using Cozy.Domain.AppCode.Extensions;
using Cozy.Domain.AppCode.Services;
using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites;
using Cozy.Domain.Models.ViewModels.ContactPostInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cozy.WebUI.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly CozyDbContext db;
        private readonly CryptoService cryptoService;
        private readonly EmailService emailService;

        public HomeController(CozyDbContext db,CryptoService cryptoService,EmailService emailService)
        {
            this.db = db;
            this.cryptoService = cryptoService;
            this.emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            
            return View();

        }

        public IActionResult Faq()
        {
            var data = db.Faqs.Where(f => f.DeletedDate == null).ToList();
            return View(data);

        }

        public IActionResult Contact()
        {
            var contactInfo = db.ContactInfos.FirstOrDefault();

            return View(new ContactPostInfoViewModel
            {
                ContactInfos = contactInfo
            });
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactPostInfoViewModel vm)
        {
            if (ModelState.IsValid)
            {
                db.ContactPosts.Add(vm.ContactPosts);

                await db.SaveChangesAsync();

                var response = new
                {
                    error = false,
                    message = "Your request has been accepted, we'll reply soon"
                };

                return Json(response);
            }

            var responseError = new
            {
                error = true,
                message = "Information is not correct, please try again",
                state = ModelState.GetError()
            };
            return Json(responseError);
        }



        [HttpPost]

        public async Task<IActionResult> Subscribe(Subscribe model)
        {

            if (model.Email == null)
            {
                return Json(new
                {
                    error = true,
                    message = "Boş göndərilə bilməz"
                });
            }

            if (!model.Email.IsEmail())
            {
                return Json(new
                {
                    error = true,
                    message = "Məlumat düzgün göndərilməyib"
                });
            }

            if (!ModelState.IsValid)
            {
                string msg = ModelState.Values.First().Errors[0].ErrorMessage;

                return Json(new
                {
                    error = true,
                    message = msg
                });
            }

            var entity = db.Subscribes.FirstOrDefault(s => s.Email.Equals(model.Email));

            if (entity != null && entity.IsApproved == true)
            {
                return Json(new
                {
                    error = false,
                    message = "Siz artıq abunə olmusunuz"
                });
            }

            if (entity == null)
            {
                db.Subscribes.Add(model);
                db.SaveChanges();
            }
            else if (entity != null)
            {
                model.Id = entity.Id;
            }

            string token = $"{model.Id}-{model.Email}-{Guid.NewGuid()}";

            token = cryptoService.Encrypt(token, true);


            string message = $"Zəhmət olmasa <a href='{Request.Scheme}://{Request.Host}/approve-subscribe?token={token}'>link</a> vasitəsilə abunəliyinizi təsdiq edin";


            await emailService.SendEmailAsync(model.Email, "Subscribe Approve Message", message);

            return Json(new
            {
                error = false,
                message = "E-mailinizə təsdiq mesajı göndərildi"
            });

        }



        [Route("/approve-subscribe")]
        public IActionResult SubscribeApprove(string token)
        {

            token = cryptoService.Decrypt(token);

            Match match = Regex.Match(token, @"^(?<id>\d+)-(?<email>[^-]+)-(?<randomKey>.*)$");



            if (match.Success)
            {
                int id = Convert.ToInt32(match.Groups["id"].Value);
                string email = match.Groups["email"].Value;
                string randomKey = match.Groups["randomKey"].Value;

                var entity = db.Subscribes.FirstOrDefault(s => s.Id == id && s.DeletedDate == null);

                if (entity == null)
                {
                    ViewBag.Message = Tuple.Create(true, "Token xətası");
                    goto end;
                }

                if (entity.IsApproved)
                {
                    ViewBag.Message = Tuple.Create(true, "Sizin müraciətiniz artıq təsdiq edilib");

                    goto end;
                }

                entity.IsApproved = true;
                entity.ApprovedDate = DateTime.UtcNow.AddHours(4);
                db.SaveChanges();


                ViewBag.Message = Tuple.Create(false, "Sizin abunəliyiniz təsdiq edildi");

            }
            else
            {
                ViewBag.Message = Tuple.Create(true, "Token xətası");
            }

        end:
            return View();


        }


        public IActionResult NotFoundPage()
        {

            return View();

        }

    }
}
