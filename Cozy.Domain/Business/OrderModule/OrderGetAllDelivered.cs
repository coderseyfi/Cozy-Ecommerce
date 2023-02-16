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
    public class OrderGetAllDeliveredQuery : IRequest<List<Order>>
    {
        public class OrderGetAllDeliveredHandler : IRequestHandler<OrderGetAllDeliveredQuery, List<Order>>
        {
            private readonly CozyDbContext db;

            public OrderGetAllDeliveredHandler(CozyDbContext db)
            {
                this.db = db;
            }


            public async Task<List<Order>> Handle(OrderGetAllDeliveredQuery request, CancellationToken cancellationToken)
            {
                var completedOrders = await db.Orders
                    .Where(o => o.DeletedDate == null && o.IsDelivered == true)
                    .ToListAsync(cancellationToken);


                return completedOrders;
            }
        }
    }
}
