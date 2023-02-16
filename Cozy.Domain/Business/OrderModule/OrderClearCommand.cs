using MediatR;
using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cozy.Domain.Business.OrderModule
{
    public class OrderClearCommand : IRequest<Order>
    {
        public int Id { get; set; }
        public class OrderClearCommandHandler : IRequestHandler<OrderClearCommand, Order>
        {
            private readonly CozyDbContext db;

            public OrderClearCommandHandler(CozyDbContext db)
            {
                this.db = db;
            }

            public async Task<Order> Handle(OrderClearCommand request, CancellationToken cancellationToken)
            {
                var data = db.Orders.FirstOrDefault(m => m.Id == request.Id && m.DeletedDate != null);

                if (data == null)
                {
                    return null;
                }

                db.Orders.Remove(data);
                await db.SaveChangesAsync(cancellationToken);
                return data;
            }
        }
    }
}

