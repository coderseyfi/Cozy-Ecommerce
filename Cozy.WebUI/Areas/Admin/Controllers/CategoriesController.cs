using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cozy.Domain.Business.CategoryModule;
using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites;

namespace Cozy.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly CozyDbContext db;
        private readonly IMediator mediator;

        public CategoriesController(CozyDbContext db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }

        [Authorize(Policy = "admin.categories.index")]
        public async Task<IActionResult> Index(CategoryGetAllQuery query)
        {
            var response = await mediator.Send(query);

            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }

        [Authorize(Policy = "admin.categories.details")]
        public async Task<IActionResult> Details(CategoryGetSingleQuery query)
        {
            var response = await mediator.Send(query);

            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }

        [Authorize(Policy = "admin.categories.create")]
        public IActionResult Create()
        {
            ViewData["ParentId"] = new SelectList(db.Categories.ToList().Where(c => c.DeletedDate == null), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.categories.create")]
        public async Task<IActionResult> Create(CategoryCreateCommand command)
        {
            if (ModelState.IsValid)
            {
                var response = await mediator.Send(command);

                if (response == null)
                {
                    return NotFound();
                }

                ViewData["ParentId"] = new SelectList(db.Categories.Where(c => c.DeletedDate == null).ToList(), "Id", "Name");

                return RedirectToAction(nameof(Index));
            }

            ViewData["ParentId"] = new SelectList(db.Categories.Where(c => c.DeletedDate == null).ToList(), "Id", "Name");
            return View(command);
        }

        [Authorize(Policy = "admin.categories.edit")]
        public async Task<IActionResult> Edit(int? id, CategoryEditCommand command)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await db.Categories
                .Include(c => c.Parent)
                .FirstOrDefaultAsync(c => c.Id == id);


            if (entity == null)
            {
                return NotFound();
            }

            ViewData["ParentId"] = new SelectList(db.Categories.Where(c => c.Id != entity.Id && c.DeletedDate == null), "Id", "Name", entity.ParentId);

            command.Id = entity.Id;
            command.Name = entity.Name;
            command.ParentId = entity.ParentId;

            return View(command);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.categories.edit")]
        public async Task<IActionResult> Edit(CategoryEditCommand command)
        {
            if (ModelState.IsValid)
            {
                var response = await mediator.Send(command);

                if (response == null)
                {
                    return NotFound();
                }

                ViewData["ParentId"] = new SelectList(db.Categories.Where(c => c.Id != command.Id && c.DeletedDate == null), "Id", "Name", command.ParentId);

                return RedirectToAction(nameof(Index));
            }

            ViewData["ParentId"] = new SelectList(db.Categories.Where(c => c.Id != command.Id && c.DeletedDate == null), "Id", "Name", command.ParentId);
            return View(command);

        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.categories.delete")]
        public async Task<IActionResult> DeleteConfirmed(CategoryRemoveCommand command)
        {
            var response = await mediator.Send(command);

            if (response == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveParent(int parentId, int id)
        {
            var category = await db.Categories.FirstOrDefaultAsync(c => c.Id == id && c.ParentId == parentId);

            category.ParentId = null;

            await db.SaveChangesAsync();

            return Json(new
            {
                error = false,
                message = "Seçilmiş kateqoriya artıq valideyn kateqoriyasına sahib deyil"
            });
        }

        private bool CategoryExists(int id)
        {
            return db.Categories.Any(e => e.Id == id);
        }
    }
}
