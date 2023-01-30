using MediatR;
using Microsoft.EntityFrameworkCore;
using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cozy.Domain.Business.ContactInfoModule
{


    public class ContactPostRemoveCommand : IRequest<ContactInfo>
    {
        public int Id { get; set; }

        public class ContactInfoRemoveCommandHandler : IRequestHandler<ContactPostRemoveCommand, ContactInfo>
        {
            private readonly CozyDbContext db;

            public ContactInfoRemoveCommandHandler(CozyDbContext db)
            {
                this.db = db;
            }

            public async Task<ContactInfo> Handle(ContactPostRemoveCommand request, CancellationToken cancellationToken)
            {
                var data = await db.ContactInfos
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
