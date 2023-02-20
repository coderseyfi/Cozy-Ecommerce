using Cozy.Domain.AppCode.Extensions;
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
    }
}
