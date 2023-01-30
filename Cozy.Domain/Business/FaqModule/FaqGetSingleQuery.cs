using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Cozy.Domain.Business.FaqModule
{
    public class FaqGetSingleQuery : IRequest<Faq>
    {
        public int Id { get; set; }

        public class FaqGetSingleQueryHandler : IRequestHandler<FaqGetSingleQuery, Faq>
        {
            private readonly CozyDbContext db;

            public FaqGetSingleQueryHandler(CozyDbContext db)
            {
                this.db = db;
            }

            public async Task<Faq> Handle(FaqGetSingleQuery request, CancellationToken cancellationToken)
            {
                var data = await db.Faqs.FirstOrDefaultAsync(p => p.Id == request.Id);

                return data;
            }
        }

    }
}
