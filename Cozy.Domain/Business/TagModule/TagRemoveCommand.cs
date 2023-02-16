using MediatR;
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

    public class TagRemoveCommand : IRequest<Tag>
    {
        public int Id { get; set; }

        public class TagRemoveCommandHandler : IRequestHandler<TagRemoveCommand, Tag>
        {
            private readonly CozyDbContext db;

            public TagRemoveCommandHandler(CozyDbContext db)
            {
                this.db = db;
            }

            public async Task<Tag> Handle(TagRemoveCommand request, CancellationToken cancellationToken)
            {
                var data = db.Tags.FirstOrDefault(m => m.Id == request.Id && m.DeletedDate == null);

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
