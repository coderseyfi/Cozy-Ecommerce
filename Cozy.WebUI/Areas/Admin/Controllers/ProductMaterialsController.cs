using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites;

namespace Cozy.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductMaterialsController : Controller
    {
        private readonly CozyDbContext db;

        public ProductMaterialsController(CozyDbContext db)
        {
            this.db = db;
        }

        // GET: Admin/ProductMaterials
        public async Task<IActionResult> Index()
        {
            return View(await db.Materials.ToListAsync());
        }

        // GET: Admin/ProductMaterials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productMaterial = await db.Materials
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productMaterial == null)
            {
                return NotFound();
            }

            return View(productMaterial);
        }

        // GET: Admin/ProductMaterials/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ProductMaterials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Id,CreatedDate,DeletedDate")] ProductMaterial productMaterial)
        {
            if (ModelState.IsValid)
            {
                db.Add(productMaterial);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productMaterial);
        }

        // GET: Admin/ProductMaterials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productMaterial = await db.Materials.FindAsync(id);
            if (productMaterial == null)
            {
                return NotFound();
            }
            return View(productMaterial);
        }

        // POST: Admin/ProductMaterials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Id,CreatedDate,DeletedDate")] ProductMaterial productMaterial)
        {
            if (id != productMaterial.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(productMaterial);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductMaterialExists(productMaterial.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(productMaterial);
        }

        // GET: Admin/ProductMaterials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productMaterial = await db.Materials
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productMaterial == null)
            {
                return NotFound();
            }

            return View(productMaterial);
        }

        // POST: Admin/ProductMaterials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productMaterial = await db.Materials.FindAsync(id);
            db.Materials.Remove(productMaterial);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductMaterialExists(int id)
        {
            return db.Materials.Any(e => e.Id == id);
        }
    }
}
