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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Cozy.Domain.Business.ProductModule
{
    public class ProductRemoveCommand : IRequest<Product>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public class ProductRemoveCommandHandler : IRequestHandler<ProductRemoveCommand, Product>
        {
            private readonly CozyDbContext db;
            private readonly IActionContextAccessor ctx;

            public ProductRemoveCommandHandler(CozyDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<Product> Handle(ProductRemoveCommand request,  CancellationToken cancellationToken)
            {
                var data = await db.Products
                   .Include(p => p.ProductCatalog.Where(i => i.DeletedDate == null))
                   .FirstOrDefaultAsync(m => m.Id == request.Id && m.DeletedDate == null, cancellationToken);

                if (data == null)
                {
                    return null;
                }

                data.DeletedDate = DateTime.UtcNow.AddHours(4);
                data.DeletedByUserId = ctx.GetCurrentUserId();

                var newPr = await db.ProductCatalogItems.Where(pc => pc.ProductId == data.Id).ToListAsync();

                foreach (var item in newPr)
                {
                    item.DeletedDate = DateTime.UtcNow.AddHours(4);
                }

                await db.SaveChangesAsync(cancellationToken);

                return data;
            }
        }
    }
}
