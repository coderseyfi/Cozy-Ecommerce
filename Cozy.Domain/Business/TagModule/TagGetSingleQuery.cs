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

namespace Cozy.Domain.Business.TagModule
{
    public class TagGetSingleQuery : IRequest<Tag>
    {
        public int Id { get; set; }

        public class TagGetSingleQueryHandler : IRequestHandler<TagGetSingleQuery, Tag>
        {
            private readonly CozyDbContext db;

            public TagGetSingleQueryHandler(CozyDbContext db)
            {
                this.db = db;
            }

            public async Task<Tag> Handle(TagGetSingleQuery request, CancellationToken cancellationToken)
            {
                var data = await db.Tags
                    .FirstOrDefaultAsync(p => p.Id == request.Id);

                return data;
            }
        }

    }
}
