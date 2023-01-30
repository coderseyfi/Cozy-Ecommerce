using MediatR;
using Microsoft.EntityFrameworkCore;
using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cozy.Domain.Business.ContactInfoModule
{
    public class ContactInfoGetSingleQuery : IRequest<ContactInfo>
    {
        public int Id { get; set; }
        public class ContactInfoGetSingleQueryHandler : IRequestHandler<ContactInfoGetSingleQuery, ContactInfo>
        {
            private readonly CozyDbContext db;

            public ContactInfoGetSingleQueryHandler(CozyDbContext db)
            {
                this.db = db;
            }

            public async Task<ContactInfo> Handle(ContactInfoGetSingleQuery request, CancellationToken cancellationToken)
            {
                var data = await db.ContactInfos.FirstOrDefaultAsync(p => p.Id == request.Id);

                return data;
            }
        }

    }
}
