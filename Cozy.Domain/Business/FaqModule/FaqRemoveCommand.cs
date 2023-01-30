using MediatR;
using Microsoft.EntityFrameworkCore;
using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using Cozy.Domain.Models.Entites;

namespace Cozy.Domain.Business.FaqModule
{


    public class FaqRemoveCommand : IRequest<Faq>
    {
        public int Id { get; set; }

        public class FaqRemoveCommandHandler : IRequestHandler<FaqRemoveCommand, Faq>
        {
            private readonly CozyDbContext db;

            public FaqRemoveCommandHandler(CozyDbContext db)
            {
                this.db = db;
            }

            public async Task<Faq> Handle(FaqRemoveCommand request, CancellationToken cancellationToken)
            {
                var data = await db.Faqs
                    .FirstOrDefaultAsync(m => m.Id == request.Id && m.DeletedDate == null, cancellationToken);

                if (data == null)
                {
                    return null;
                }

                data.DeletedDate = DateTime.UtcNow.AddHours(4);
                await db.SaveChangesAsync(cancellationToken);

                return data;
            }


        }
    }
}
