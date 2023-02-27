using Cozy.Domain.Business.BlogPostModule;
using Cozy.Domain.Models.DataContexts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Cozy.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogPostsController : Controller
    {
        private readonly CozyDbContext db;

        public IMediator mediator { get; }

        public BlogPostsController(CozyDbContext db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }

        [Authorize(Policy = "admin.blogposts.index")]
        public async Task<IActionResult> Index(BlogPostGetAllQueryAdmin query)
        {
            var response = await mediator.Send(query);

            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }

        [Authorize(Policy = "admin.blogposts.details")]
        public async Task<IActionResult> Details(BlogPostSingleQuery query)
        {
            var response = await mediator.Send(query);

            if (response == null)
            {
                return NotFound();
            }
            return View(response);
        }


        [Authorize(Policy = "admin.blogposts.create")]
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.DeletedDate == null).ToList(), "Id", "Name");
            ViewBag.Tags = new SelectList(db.Tags.Where(p => p.DeletedDate == null).ToList(), "Id", "Text");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.blogposts.create")]
        public async Task<IActionResult> Create(BlogPostCreateCommand command)
        {

            if (command.Image == null)
            {
                ModelState.AddModelError("ImagePath", "Blog image should not be left empty");
            }

            if (ModelState.IsValid)
            {
                var response = await mediator.Send(command);

                ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.DeletedDate == null).ToList(), "Id", "Name", command.CategoryId);
                ViewBag.Tags = new SelectList(db.Tags.Where(p => p.DeletedDate == null).ToList(), "Id", "Text");
                return RedirectToAction(nameof(Index));
            }


            ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.DeletedDate == null).ToList(), "Id", "Name", command.CategoryId);
            ViewBag.Tags = new SelectList(db.Tags.Where(p => p.DeletedDate == null).ToList(), "Id", "Text");

            return View(command);
        }

        [Authorize(Policy = "admin.blogposts.edit")]
        public async Task<IActionResult> Edit(int? id, BlogPostEditCommand command)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await db.BlogPosts
                .Include(bp => bp.Category)
                .Include(bp => bp.TagCloud)
                .FirstOrDefaultAsync(bp => bp.Id == id);
            if (entity == null)
            {
                return NotFound();
            }

            ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.DeletedDate == null).ToList(), "Id", "Name", entity.CategoryId);
            ViewBag.Tags = new SelectList(db.Tags.Where(p => p.DeletedDate == null).ToList(), "Id", "Text");


            command.Id = entity.Id;
            command.Title = entity.Title;
            command.Body = entity.Body;
            command.ImagePath = entity.ImagePath;
            command.CategoryId = entity.CategoryId;
            command.TagIds = entity.TagCloud.Select(tc => tc.TagId).ToArray();


            return View(command);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.blogposts.edit")]
        public async Task<IActionResult> Edit(int id, BlogPostEditCommand command)
        {
            if (ModelState.IsValid)
            {
                if (id != command.Id)
                {
                    return NotFound();
                }

                var response = await mediator.Send(command);

                if (response == null)
                {
                    ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.DeletedDate == null).ToList(), "Id", "Name", command.CategoryId);
                    ViewBag.Tags = new SelectList(db.Tags.Where(p => p.DeletedDate == null).ToList(), "Id", "Text");
                    return RedirectToAction(nameof(Index));
                }
            }


            ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.DeletedDate == null).ToList(), "Id", "Name", command.CategoryId);
            ViewBag.Tags = new SelectList(db.Tags.Where(p => p.DeletedDate == null).ToList(), "Id", "Text");
            return View(command);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.blogposts.delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, BlogPostRemoveCommand command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            var response = await mediator.Send(command);

            if (response == null)
            {
                return NotFound();
            }


            return RedirectToAction(nameof(Index));
        }


        [HttpPost, ActionName("Publish")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.blogposts.publish")]
        public async Task<IActionResult> PublishConfirmed(int id, BlogPostPublishCommand command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            var response = await mediator.Send(command);

            if (response == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }




        [Authorize(Policy = "admin.blogposts.deletedposts")]
        public async Task<IActionResult> DeletedPosts(BlogPostGetAllDeletedQuery query)
        {
            var response = await mediator.Send(query);
            return View(response);
        }



        [HttpPost, ActionName("Back")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.blogposts.backtoposts")]
        public async Task<IActionResult> BackToPosts(int id, BlogPostRemoveBackCommand command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            var response = await mediator.Send(command);

            return RedirectToAction(nameof(Index));
        }


        [Authorize(Policy = "admin.blogposts.deletedpostdetails")]
        public async Task<IActionResult> DeletedPostDetails(BlogPostGetDeletedSingleQuery query)
        {
            var response = await mediator.Send(query);

            if (response == null)
            {
                return NotFound();
            }
            return View(response);
        }



        [HttpPost, ActionName("Clear")]
        [Authorize(Policy = "admin.blogposts.cleardeletedposts")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClearDeletedPosts(int id, BlogPostClearCommand command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            var response = await mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }



        [Authorize(Policy = "admin.blogposts.getcomments")]
        public async Task<IActionResult> GetComments(BlogPostGetCommentsQuery query)
        {
            var response = await mediator.Send(query);

            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }


        [Authorize(Policy = "admin.blogposts.getlikes")]
        public async Task<IActionResult> GetLikes(BlogPostGetLikesQuery query)
        {
            var response = await mediator.Send(query);

            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }



        [Authorize(Policy = "admin.blogposts.commentdetails")]
        public async Task<IActionResult> CommentDetails(BlogPostGetSingleCommentQuery query)
        {
            var response = await mediator.Send(query);

            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }



        [HttpPost, ActionName("CommentDelete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.blogposts.commentdelete")]
        public async Task<IActionResult> CommentDeleteConfirmed(int id, BlogPostCommentRemoveCommand command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            var response = await mediator.Send(command);

            if (response == null)
            {
                return NotFound();
            }


            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> RemoveCategory(int categoryId, int id)
        {
            var blogPostt = await db.BlogPosts.FirstOrDefaultAsync(c => c.Id == id && c.CategoryId == categoryId);

            blogPostt.CategoryId = null;

            await db.SaveChangesAsync();

            return Json(new
            {
                error = false,
                message = "The selected blog post does not have any category anymore"
            });
        }

        private bool BlogPostExists(int id)
        {
            return db.BlogPosts.Any(e => e.Id == id);
        }
    }
}
