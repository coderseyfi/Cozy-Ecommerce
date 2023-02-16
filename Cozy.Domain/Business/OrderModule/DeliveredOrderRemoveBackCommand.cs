using MediatR;
using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cozy.Domain.Business.OrderModule
{
    public class DeliveredOrderRemoveBackCommand : IRequest<Order>
    {
        public int Id { get; set; }
        public class DeliveredOrderRemoveBackCommandHandler : IRequestHandler<DeliveredOrderRemoveBackCommand, Order>
        {
            private readonly CozyDbContext db;

            public DeliveredOrderRemoveBackCommandHandler(CozyDbContext db)
            {
                this.db = db;
            }

            public async Task<Order> Handle(DeliveredOrderRemoveBackCommand request, CancellationToken cancellationToken)
            {
                var data = db.Orders.FirstOrDefault(m => m.Id == request.Id && m.IsDelivered == true);

                if (data == null)
                {
                    return null;
                }

                data.IsDelivered = false;

                await db.SaveChangesAsync(cancellationToken);
                return data;
            }
        }
    }
}

