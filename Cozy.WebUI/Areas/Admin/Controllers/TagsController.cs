using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cozy.Domain.Business.TagModule;
using Cozy.Domain.Models.DataContexts;
using System.Linq;
using System.Threading.Tasks;

namespace Cozy.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagsController : Controller
    {
        private readonly CozyDbContext db;
        private readonly IMediator mediator;

        public TagsController(CozyDbContext db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }

        [Authorize(Policy = "admin.tags.index")]
        public async Task<IActionResult> Index(TagGetAllQuery query)
        {
            var response = await mediator.Send(query);

            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }

        [Authorize(Policy = "admin.tags.details")]
        public async Task<IActionResult> Details(TagGetSingleQuery query)
        {
            var response = await mediator.Send(query);

            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }

        [Authorize(Policy = "admin.tags.create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.tags.create")]
        public async Task<IActionResult> Create(TagCreateCommand command)
        {
            if (ModelState.IsValid)
            {
                var response = await mediator.Send(command);

                if (response == null)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(command);
        }

        [Authorize(Policy = "admin.tags.edit")]
        public async Task<IActionResult> Edit(int? id, TagEditCommand command)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await db.Tags
                .FirstOrDefaultAsync(c => c.Id == id);


            if (entity == null)
            {
                return NotFound();
            }


            command.Id = entity.Id;
            command.Text = entity.Text;

            return View(command);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.tags.edit")]
        public async Task<IActionResult> Edit(TagEditCommand command)
        {
            if (ModelState.IsValid)
            {
                var response = await mediator.Send(command);

                if (response == null)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(command);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.tags.delete")]
        public async Task<IActionResult> DeleteConfirmed(TagRemoveCommand command)
        {
            var response = await mediator.Send(command);

            if (response == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TagExists(int id)
        {
            return db.Tags.Any(e => e.Id == id);
        }
    }
}
