using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cozy.Domain.Models.ViewComponents
{
    public class SearchPanelViewComponent : ViewComponent
    {
        private readonly CozyDbContext db;

        public SearchPanelViewComponent(CozyDbContext db)
        {
            this.db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var vm = new SearchPanelViewModel();

            vm.Colors = await db.ProductCatalogItems
                .Include(pc => pc.Color)
                .Select(pc => pc.Color)
                .Where(pc => pc.DeletedDate == null)
                .Distinct()
                .ToArrayAsync();



            vm.Materials = await db.ProductCatalogItems
                .Include(pc => pc.Material)
                .Select(pc => pc.Material)
                .Where(pc => pc.DeletedDate == null)
                .Distinct()
                .ToArrayAsync();



            vm.Brands = await db.ProductCatalogItems
                .Include(pc => pc.Product)
                .ThenInclude(pc => pc.Brand)
                .Select(pc => pc.Product.Brand)
                .Where(pc => pc.DeletedDate == null)
                .Distinct()
                .ToArrayAsync();

            vm.Categories = await db.ProductCatalogItems
               .Include(pc => pc.Product)
               .ThenInclude(pc => pc.Category)
               .Select(pc => pc.Product.Category)
               .Where(pc => pc.DeletedDate == null)
               .Distinct()
               .ToArrayAsync();

            var priceRange = await db.ProductCatalogItems
                .Include(pc => pc.Product)
                .Select(pc => pc.Product.Price)
                .ToArrayAsync();



            vm.Min = (int)Math.Floor(priceRange.Min());
            vm.Max = (int)Math.Ceiling(priceRange.Max());

            return View(vm);
        }
    }
}


