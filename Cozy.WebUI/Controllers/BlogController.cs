using Cozy.Domain.AppCode.Extensions;
using Cozy.Domain.Business.BlogPostModule;
using Cozy.Domain.Models.DataContexts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cozy.WebUI.Controllers
{
    public class BlogController : Controller
    {
        private readonly CozyDbContext db;
        private readonly IMediator mediator;

        public BlogController(CozyDbContext db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(BlogPostGetAllQuery query)
        {
            var response = await mediator.Send(query);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_PostBody", response);
            }
            return View(response);
        }


        [AllowAnonymous]
        [Route("/blog/{slug}")]
        public async Task<IActionResult> Details(BlogPostSingleQuery query)
        {
            var entity = await mediator.Send(query);

            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        [HttpPost]
        [Route("/post-comment")]
        public async Task<IActionResult> PostComment(BlogPostCommentCommand command)
        {

            try
            {

                var response = await mediator.Send(command);

                return PartialView("_Comments", response);

            }
            catch (System.Exception ex)
            {

                return Json(new
                {
                    error = true,
                    message = ex.Message
                });

            }

        }
    }
}
