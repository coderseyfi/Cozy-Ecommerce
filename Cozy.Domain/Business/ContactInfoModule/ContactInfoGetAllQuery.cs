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
    
    public class ContactInfoGetAllQuery : IRequest<List<ContactInfo>>
    {
        public class ContactInfoGetAllQueryHandler : IRequestHandler<ContactInfoGetAllQuery, List<ContactInfo>>
        {
            private readonly CozyDbContext db;

            public ContactInfoGetAllQueryHandler(CozyDbContext db)
            {
                this.db = db;
            }
            public async Task<List<ContactInfo>> Handle(ContactInfoGetAllQuery request, CancellationToken cancellationToken)
            {
                var data = await db.ContactInfos
                .Where(bp => bp.DeletedDate == null)
                .ToListAsync(cancellationToken);

                return data;
            }
        }


    }
}
