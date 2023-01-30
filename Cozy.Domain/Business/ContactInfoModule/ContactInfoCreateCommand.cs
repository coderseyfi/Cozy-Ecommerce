using MediatR;
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
    public class ContactInfoCreateCommand : IRequest<ContactInfo>
    {
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string EmailAddress { get; set; }


        public class ContactInfoCreateCommandHandler : IRequestHandler<ContactInfoCreateCommand, ContactInfo>
        {
            private readonly CozyDbContext db;

            public ContactInfoCreateCommandHandler(CozyDbContext db)
            {
                this.db = db;
            }

            public async Task<ContactInfo> Handle(ContactInfoCreateCommand request, CancellationToken cancellationToken)
            {
                var model = new ContactInfo();

                model.PhoneNumber = request.PhoneNumber;

                model.Location = request.Location;

                model.EmailAddress = request.EmailAddress;

                await db.ContactInfos.AddAsync(model, cancellationToken);

                await db.SaveChangesAsync(cancellationToken);

                return model;
            }


        }
    }

}
