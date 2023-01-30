using MediatR;
using Microsoft.EntityFrameworkCore;
using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cozy.Domain.Business.ContactInfoModule
{

    public class ContactInfoEditCommand : IRequest<ContactInfo>
    {
        public int Id { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Location { get; set; }


        [Required]
        public string EmailAddress { get; set; }

        public class ContactInfoEditCommandHandler : IRequestHandler<ContactInfoEditCommand, ContactInfo>
        {
            private readonly CozyDbContext db;

            public ContactInfoEditCommandHandler(CozyDbContext db)
            {
                this.db = db;
            }

            public async Task<ContactInfo> Handle(ContactInfoEditCommand request, CancellationToken cancellationToken)
            {
                var data = await db.ContactInfos
                    .FirstOrDefaultAsync(m => m.Id == request.Id && m.DeletedDate == null, cancellationToken);

                if (data == null)
                {
                    return null;
                }

                data.PhoneNumber = request.PhoneNumber;

                data.Location = request.Location;

                data.EmailAddress = request.EmailAddress;

                await db.SaveChangesAsync(cancellationToken);

                return data;
            }


        }
    }
}
