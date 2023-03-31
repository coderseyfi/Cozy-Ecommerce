using Azure.Core;
using Cozy.Domain.AppCode.Extensions;
using Cozy.Domain.AppCode.Infrastructure;
using Cozy.Domain.Business.BasketModule;
using Cozy.Domain.Business.ProductModule;
using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites;
using Cozy.Domain.Models.ViewModels.OrderViewModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cozy.WebUI.Controllers
{


    public class ShopController : Controller
    {
        private readonly CozyDbContext db;
        private readonly IMediator mediator;

        public ShopController(CozyDbContext db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }


        [AllowAnonymous]
        public async Task<IActionResult> Index(ProductFilterQuery query)
        {
            var response = await mediator.Send(query);

            if (Request.IsAjaxRequest())
            {
                //return Json(response);
                return PartialView("_Products", response);
            }

            return View(response);
        }


        [AllowAnonymous]
        public IActionResult SortBy(string sortOrder, ProductsPagedQuery request)
        {
            var products = db.Products
                .Include(p => p.ProductImages.Where(pi => pi.IsMain == true))
                .Include(p => p.Category)
                .Where(p => p.DeletedDate == null)
                .OrderByDescending(p => p.CreatedDate)
                .AsQueryable();

            var pagedDate = new PagedViewModel<Product>(products, request);


            switch (sortOrder)
            {
                case "priceDesc":
                    products = products.OrderByDescending(p => p.Price);
                    pagedDate = new PagedViewModel<Product>(products, request);
                    break;
                case "priceAsc":
                    products = products.OrderBy(p => p.Price);
                    pagedDate = new PagedViewModel<Product>(products, request);
                    break;
                case "rateDesc":
                    products = products.OrderByDescending(p => p.Rate);
                    pagedDate = new PagedViewModel<Product>(products, request);
                    break;
                default:
                    products = products.OrderByDescending(p => p.CreatedDate);
                    pagedDate = new PagedViewModel<Product>(products, request);
                    break;
            }

            return PartialView("_Products", pagedDate);
        }



        [AllowAnonymous]
        public async Task<IActionResult> Details(ProductSingleQuery query)
        {

            var product = await mediator.Send(query);


            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        [AllowAnonymous]
        [Route("/wishlist")]
        public async Task<IActionResult> Wishlist(WishlistQuery query)
        {
            var favs = await mediator.Send(query);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_WishlistBody", favs);
            }

            return View(favs);
        }



        

        [Route("/basket")]
        public async Task<IActionResult> Basket(ProductBasketQuery query)
        {
            var response = await mediator.Send(query);

            return View(response);
        }




        [HttpPost]
        [Route("/basket")]
        public async Task<IActionResult> Basket(AddToBasketCommand command)
        {
            var response = await mediator.Send(command);

            return Json(response);
        }



        [HttpPost]
        public async Task<IActionResult> RemoveFromBasket(RemoveFromBasketCommand command)
        {
            var response = await mediator.Send(command);

            return Json(response);
        }



        [HttpPost]
        public async Task<IActionResult> ChangeBasketQuantity(ChangeBasketQuantityCommand command)
        {
            var response = await mediator.Send(command);

            return Json(response);
        }


        [Route("/checkout")]
        public async Task<IActionResult> Checkout(ProductBasketQuery query)
        {
            var response = await mediator.Send(query);

            return View(new OrderViewModel
            {
                BasketProducts = response
            });
        }

        [HttpPost]
        [Route("/checkout")]
        public async Task<IActionResult> Checkout(OrderViewModel vm, int[] productIds, int[] quantities)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(vm.OrderDetails);

                await db.SaveChangesAsync();

                vm.OrderDetails.OrderProducts = new List<OrderProduct>();

                for (int i = 0; i < productIds.Length; i++)
                {
                    var product = db.Products.Find(productIds[i]);
                    vm.OrderDetails.OrderProducts.Add(new OrderProduct
                    {
                        OrderId = vm.OrderDetails.Id,
                        ProductId = product.Id,
                        Quantity = quantities[i]
                    });
                }
                await db.SaveChangesAsync();

                var response = new
                {
                    error = false,
                    message = "Your order was completed"
                };

                return Json(response);
            }

            var responseError = new
            {
                error = true,
                message = "The error was occurred while completing your order",
                state = ModelState.GetError()
            };
            return Json(responseError);
        }


        [HttpPost]
        public async Task<IActionResult> SetProductRate(SetRateCommand command)
        {
            var response = await mediator.Send(command);

            return Json(response);
        }



        public async Task<IActionResult> SearchProducts(string searchTerm)
        {
            var products = await mediator.Send(new SearchProductQuery { SearchTerm = searchTerm });

            if (Request.IsAjaxRequest())
            {
                return PartialView("_SearchResults", products.OrderByDescending(p => p.CreatedDate));
            }

            return View(products);


            //var brands = await db.Brands.Where(b => b.DeletedDate == null).ToListAsync();

            //var categories = await db.Categories
            //    .Include(c => c.Children)
            //    .ThenInclude(c => c.Children)
            //    .ThenInclude(c => c.Children)
            //    .ThenInclude(c => c.Children)
            //    .Where(c => c.DeletedDate == null && c.ParentId == null).ToListAsync();

            //var maxPrice = Math.Ceiling(db.Products.Where(p => p.DeletedDate == null).Select(p => p.Price).Max());

            //var vm = new ProductViewModel()
            //{
            //    Brands = brands,
            //    Categories = categories,
            //    Products = products.ToList(),
            //    MaxPrice = maxPrice
            ////};


        }
    }
}
