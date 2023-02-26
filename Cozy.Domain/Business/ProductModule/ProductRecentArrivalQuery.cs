using MediatR;
using Microsoft.EntityFrameworkCore;
using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cozy.Domain.Business.ProductModule
{
    public class ProductRecentArrivalQuery : IRequest<List<Product>>
    {

        public int Size { get; set; }
        public int PostId { get; set; }

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
                     .Where(bp => bp.DeletedDate == null && bp.CreatedDate != null)
                     .OrderByDescending(bp => bp.CreatedDate)
                     .Take(request.Size < 4 ? 4 : request.Size)
                     .ToListAsync(cancellationToken);

                return posts;
            }
        }

    }
}