using Cozy.Domain.AppCode.Extensions;
using Cozy.Domain.AppCode.Infrastructure;
using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cozy.Domain.Business.ProductModule
{
    public class AddToBasketCommand : IRequest<JsonResponse>
    {
        public int ProductId { get; set; }

        public class AddToBasketCommandHandler : IRequestHandler<AddToBasketCommand, JsonResponse>
        {
            private readonly CozyDbContext db;
            private readonly IActionContextAccessor ctx;

            public AddToBasketCommandHandler(CozyDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }


            public async Task<JsonResponse> Handle(AddToBasketCommand request, CancellationToken cancellationToken)
            {
                var userId = ctx.GetCurrentUserId();


                //if (userId == 0)
                //{
                //    return new JsonResponse
                //    {
                //        Error = true,
                //        Message = "You should login to your account first"
                //    };
                //}

                var alreadyExists = await db.Basket.AnyAsync(b => b.ProductId == request.ProductId && b.UserId == userId, cancellationToken);

                if (alreadyExists)
                {
                    return new JsonResponse
                    {
                        Error = true,
                        Message = "Product already exist in your basket"
                    };
                }


                var basketItem = new Basket
                {
                    UserId = userId,
                    ProductId = request.ProductId,
                    Quantity = 1
                };

                await db.Basket.AddAsync(basketItem, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                #region Baskete elave olunandan sonra wishlistden silinme
                //var value = ctx.GetIntArrayFromCookie("favorites");
                //if (value != null)
                //{
                //    var newFavIds = value.Where(fi => fi != request.ProductId);
                //    var cookieValue = string.Join(",", newFavIds);
                //    var cookie = new KeyValuePair<string, string>("favorites", cookieValue);
                //    ctx.ActionContext.HttpContext.Request.Cookies.Append(cookie);
                //    ctx.SetValueToCookie("favorites", cookieValue);
                //}

                #endregion


                return new JsonResponse
                {
                    Error = false,
                    Message = "Product was added to the basket"
                };
            }
        }
    }
}