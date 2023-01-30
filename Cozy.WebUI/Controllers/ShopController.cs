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
            var categories = await db.Categories.Where(c => c.DeletedDate == null).ToListAsync();
            var colors = await db.Colors.Where(c => c.DeletedDate == null).ToListAsync();


            var vm = new ProductViewModel()
            {
                Brands = brands,
                Categories = categories,
                Colors = colors
            };
          
           


            return View(vm);
        }

        public IActionResult Details()
        {
            return View();
        }
    }
}
