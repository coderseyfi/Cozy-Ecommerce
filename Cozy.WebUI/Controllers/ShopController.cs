using Cozy.Domain.Models.DataContexts;
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
                Products = products
            };
            
           


            return View(vm);
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
