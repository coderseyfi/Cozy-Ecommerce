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
    public class TopRatedProductQuery : IRequest<List<Product>>
    {

        public class TopRatedProductQueryHandler : IRequestHandler<TopRatedProductQuery, List<Product>>
        {
            private readonly CozyDbContext db;

            public TopRatedProductQueryHandler(CozyDbContext db)
            {
                this.db = db;
            }


            public async Task<List<Product>> Handle(TopRatedProductQuery request, CancellationToken cancellationToken)
            {
                var products = await db.Products
                     .Include(p => p.ProductImages.Where(pi => pi.IsMain == true))
                     .Where(p => p.Rate > 3 && p.DeletedDate == null)
                     .Take(6)
                     .OrderByDescending(bp => bp.Rate)
                     .ToListAsync(cancellationToken);

                return products;
            }
        }

    }
}