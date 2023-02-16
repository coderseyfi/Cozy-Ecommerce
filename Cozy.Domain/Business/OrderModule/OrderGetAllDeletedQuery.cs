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
    public class OrderGetAllCancelledQuery : IRequest<List<Order>>
    {
        public class OrderGetAllCancelledHandler : IRequestHandler<OrderGetAllCancelledQuery, List<Order>>
        {
            private readonly CozyDbContext db;

            public OrderGetAllCancelledHandler(CozyDbContext db)
            {
                this.db = db;
            }


            public async Task<List<Order>> Handle(OrderGetAllCancelledQuery request, CancellationToken cancellationToken)
            {
                var completedOrders = await db.Orders
                    .Where(o => o.DeletedDate != null)
                    .ToListAsync(cancellationToken);


                return completedOrders;
            }
        }
    }
}
