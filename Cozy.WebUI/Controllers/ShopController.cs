using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.FormModels;
using Cozy.Domain.Models.ViewModels.ProductViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Cozy.WebUI.Controllers
{

    [AllowAnonymous]
    public class ShopController : Controller
    {
        private readonly CozyDbContext db;

        public ShopController(CozyDbContext db)
        {
            this.db = db;
        }

        public async Task<IActionResult> Index()
        {
            var brands = await db.Brands.Where(b => b.DeletedDate == null).ToListAsync();

            var categories = await db.Categories
                .Include(c => c.Children)
                .ThenInclude(c => c.Children)
                .Where(b => b.DeletedDate == null && b.ParentId == null)
                .ToListAsync();

            var colors = await db.Colors.Where(c => c.DeletedDate == null).ToListAsync();
            var materials = await db.Materials.Where(c => c.DeletedDate == null).ToListAsync();

            var products = await db.Products
                .Include(p=>p.ProductImages.Where(i=>i.IsMain == true))
                .Include(c => c.Brand)
                .Where(b => b.DeletedDate == null)
                .ToListAsync();


            var vm = new ProductViewModel()
            {
                Brands = brands,
                Categories = categories,
                Colors = colors,
                Materials= materials,
                Products = products
            };
            
           


            return View(vm);
        }



        [HttpPost]
        public IActionResult Filter(ShopFilterFormModel model)
        {
            var query = db.Products
                .Include(p => p.ProductImages.Where(i => i.IsMain == true))
                //.Include(c=>c.Category)
                .Include(p => p.Brand)
                .Where(p => p.DeletedDate == null)
                .AsQueryable();

            if (model?.Brands?.Count() > 0)
            {
                query = query.Where(p => model.Brands.Contains(p.BrandId));
            }

            if (model?.Colors?.Count() > 0)
            {
                query = query.Where(p => model.Colors.Contains(p.BrandId));
            }

            //if (model?.Categories?.Count() > 0)
            //{
            //    query = query.Where(p => model.Categories.Contains(p.CategoryId));
            //}

            //if (model.Prices[0] >= 0 && model.Prices[0] <= model.Prices[1])
            //{
            //    query = query.Where(q => q.Price >= model.Prices[0] && q.Price <= model.Prices[1]);
            //}

            return PartialView("_ProductsContainer", query.ToList());
        }



        public async Task<IActionResult> Details(int id)
        {
            
           var product = await db.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == id && p.DeletedDate == null);


            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}
