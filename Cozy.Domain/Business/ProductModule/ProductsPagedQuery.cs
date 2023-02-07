using Cozy.Domain.AppCode.Infrastructure;
using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cozy.Domain.Business.ProductModule
{
    public class ProductsPagedQuery :PaginateModel, IRequest<PagedViewModel<Product>>
    {
        public class ProductsPagedQueryHandler : IRequestHandler<ProductsPagedQuery, PagedViewModel<Product>>
        {
            private readonly CozyDbContext db;

            public ProductsPagedQueryHandler(CozyDbContext db)
            {
                this.db = db;
            }

            public async Task<PagedViewModel<Product>> Handle(ProductsPagedQuery request, CancellationToken cancellationToken)
            {
                var query = db.Products
                    .Include(p=>p.ProductImages)
                    .Include(p=>p.Brand)
                    .Include(p=>p.Category)
                
                    .Where(m => m.DeletedDate == null)
                    .OrderByDescending(p=>p.Id)
                    .AsQueryable();

                var pagedDate = new PagedViewModel<Product>(query, request);

                return pagedDate;
            }
        }
    }
}
