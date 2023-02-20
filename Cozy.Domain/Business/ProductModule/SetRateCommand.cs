using Cozy.Domain.AppCode.Extensions;
using Cozy.Domain.AppCode.Infrastructure;
using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cozy.Domain.Business.ProductModule
{
    public class SetRateCommand : IRequest<JsonResponse>
    {
        public int ProductId { get; set; }
        public byte Rate { get; set; }

        public class SetRateCommandHandler : IRequestHandler<SetRateCommand, JsonResponse>
        {
            private readonly CozyDbContext db;
            private readonly IActionContextAccessor ctx;

            public SetRateCommandHandler(CozyDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }

            public async Task<JsonResponse> Handle(SetRateCommand request, CancellationToken cancellationToken)
            {
                var userId = ctx.GetCurrentUserId();

                var rateEntry = await db.ProductRates.FirstOrDefaultAsync(m => m.ProductId == request.ProductId
                && m.CreatedByUserId == userId, cancellationToken);

                if (rateEntry != null)
                {
                    rateEntry.Rate = request.Rate;
                    rateEntry.CreatedDate = DateTime.Now;
                    await db.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    rateEntry = new ProductRate
                    {
                        ProductId = request.ProductId,
                        Rate = request.Rate,
                        CreatedByUserId = userId,
                        CreatedDate = DateTime.Now
                    };
                    await db.ProductRates.AddAsync(rateEntry, cancellationToken);
                    await db.SaveChangesAsync(cancellationToken);
                }

                var avgRate = db.ProductRates.Where(m => m.ProductId == request.ProductId).Average(m => m.Rate);

                var product = await db.Products.FirstOrDefaultAsync(m => m.Id == request.ProductId, cancellationToken);

                product.Rate = avgRate;
                await db.SaveChangesAsync(cancellationToken);

                return new JsonResponse
                {
                    Error = false,
                    Message = "ok",
                    Value = avgRate
                };
            }
        }
    }
}