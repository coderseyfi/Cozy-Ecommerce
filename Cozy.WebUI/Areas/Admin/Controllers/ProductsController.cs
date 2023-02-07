using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites;
using MediatR;
using Cozy.Domain.Business.ProductModule;
using Cozy.Domain.Business.BrandModule;
using Microsoft.AspNetCore.Authorization;
using Cozy.Domain.AppCode.Extensions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Cozy.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly CozyDbContext db;
        private readonly IMediator mediator;

        public ProductsController(CozyDbContext db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }

        public async Task<IActionResult> Index(ProductsPagedQuery query)
        {
            var response = await mediator.Send(query);

            return View(response);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await db.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

      
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(db.Brands, "Id", "Name");
            ViewData["CategoryId"] = new SelectList(db.Categories, "Id", "Name");
            ViewData["CatalogId"] = new SelectList(db.ProductCatalogItems, "Id", "Name");

            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateCommand command)
        {

            var response = await mediator.Send(command);

            if (response == null)
            {

                ViewBag.BrandId = new SelectList(db.Brands, "Id", "Name", command.BrandId);
                ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", command.CategoryId);
                ViewData["CatalogId"] = new SelectList(db.ProductCatalogItems, "Id", "Name");
                return View(command);
            }

            return RedirectToAction(nameof(Index));
            
        }

        public async Task<IActionResult> Edit(ProductSingleQuery query)
        {
            var product = await mediator.Send(query);

            if (product == null)
            {
                return NotFound();
            }

            var command = new ProductEditCommand();
            command.Name = product.Name;
            command.Price = product.Price;
            command.ShortDescription = product.ShortDescription;
            command.Description = product.Description;
            command.BrandId = product.BrandId;
            command.CategoryId = product.CategoryId;
            command.StockKeepingUnit = product.StockKeepingUnit;

            command.Images = product.ProductImages.Select(x=> new ImageItem
            {
                    Id = x.Id,
                    TempPath = x.Name,
                    IsMain= x.IsMain,


            }).ToArray();

            ViewData["BrandId"] = new SelectList(db.Brands, "Id", "Name", product.BrandId);
            ViewData["CategoryId"] = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(command);
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,ProductEditCommand command)
        {
         
           
            if (id != command.Id)
            {
                return NotFound();
            }

            var response = await mediator.Send(command);

            if (response == null)
            {

                ViewData["BrandId"] = new SelectList(db.Brands, "Id", "Name", command.BrandId);
                ViewData["CategoryId"] = new SelectList(db.Categories, "Id", "Name", command.CategoryId);
                return View(command);

            }

            return RedirectToAction(nameof(Index));
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.products.delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await db.Products.FirstOrDefaultAsync(p => p.Id == id && p.DeletedDate == null);

            if (product == null)
            {
                return Json(new
                {
                    error = true,
                    message = "Melumat movcud deyil"
                });
            }

            product.DeletedDate = DateTime.UtcNow.AddHours(4);
            product.DeletedByUserId = User.GetCurrentUserIdNew();
            await db.SaveChangesAsync();

            var response = await mediator.Send(new ProductsPagedQuery());

            return PartialView("_ListBody", response);

           
        }



        private bool ProductExists(int id)
        {
            return db.Products.Any(e => e.Id == id);
        }
    }
}
