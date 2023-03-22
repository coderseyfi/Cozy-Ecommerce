using MediatR;
using Microsoft.EntityFrameworkCore;
using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Cozy.Domain.Business.ProductModule
{
    public class ProductRecentArrivalQuery : IRequest<List<Product>>
    {

        public int Size { get; set; }

        public class ProductRecentQueryHandler : IRequestHandler<ProductRecentArrivalQuery, List<Product>>
        {
            private readonly CozyDbContext db;

            public ProductRecentQueryHandler(CozyDbContext db)
            {
                this.db = db;
            }
            public async Task<List<Product>> Handle(ProductRecentArrivalQuery request, CancellationToken cancellationToken)
            {
                var posts = await db.Products
                     .Where(bp => bp.DeletedDate == null && bp.CreatedDate > DateTime.Now.AddDays(-7))
                     .Include(p => p.ProductImages)
                     .OrderByDescending(bp => bp.CreatedDate)
                     .Take(8)
                     .ToListAsync(cancellationToken);

                return posts;
            }
        }

    }
}