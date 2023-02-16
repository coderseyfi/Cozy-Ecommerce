using MediatR;
using Microsoft.EntityFrameworkCore;
using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cozy.Domain.Business.OrderModule
{

    public class OrderGetAllQuery : IRequest<List<Order>>
    {
        public class OrderGetAllQueryHandler : IRequestHandler<OrderGetAllQuery, List<Order>>
        {
            private readonly CozyDbContext db;

            public OrderGetAllQueryHandler(CozyDbContext db)
            {
                this.db = db;
            }
            public async Task<List<Order>> Handle(OrderGetAllQuery request, CancellationToken cancellationToken)
            {
                var data = await db.Orders
                .Where(o => o.DeletedDate == null && o.IsDelivered == false)
                .ToListAsync(cancellationToken);

                return data;
            }
        }


    }
}
