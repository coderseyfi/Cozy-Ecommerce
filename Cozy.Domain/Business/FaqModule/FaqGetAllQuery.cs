using MediatR;
using Microsoft.EntityFrameworkCore;
using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cozy.Domain.Models.Entites;

namespace Cozy.Domain.Business.FaqModule
{
    
    public class FaqGetAllQuery : IRequest<List<Faq>>
    {
        public class FaqGetAllQueryHandler : IRequestHandler<FaqGetAllQuery, List<Faq>>
        {
            private readonly CozyDbContext db;

            public FaqGetAllQueryHandler(CozyDbContext db)
            {
                this.db = db;
            }
            public async Task<List<Faq>> Handle(FaqGetAllQuery request, CancellationToken cancellationToken)
            {
                var data = await db.Faqs
                .Where(bp => bp.DeletedDate == null)
                .ToListAsync(cancellationToken);

                return data;
            }
        }


    }
}
