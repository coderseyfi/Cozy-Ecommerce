using MediatR;
using Microsoft.EntityFrameworkCore;
using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Cozy.Domain.Business.TagModule
{

    public class TagEditCommand : IRequest<Tag>
    {
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }


        public class TagEditCommandHandler : IRequestHandler<TagEditCommand, Tag>
        {
            private readonly CozyDbContext db;

            public TagEditCommandHandler(CozyDbContext db)
            {
                this.db = db;
            }

            public async Task<Tag> Handle(TagEditCommand request, CancellationToken cancellationToken)
            {
                var entity = await db.Tags
                       .FirstOrDefaultAsync(bp => bp.Id == request.Id && bp.DeletedDate == null);
                if (entity == null)
                {
                    return null;
                }

                entity.Text = request.Text;

                await db.SaveChangesAsync(cancellationToken);

                return entity;
            }


        }
    }
}
