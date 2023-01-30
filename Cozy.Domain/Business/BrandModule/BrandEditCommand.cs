using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Cozy.Domain.Business.BrandModule
{

    public class BrandEditCommand : IRequest<Brand>
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }


        public class BrandEditCommandHandler : IRequestHandler<BrandEditCommand, Brand>
        {
            private readonly CozyDbContext db;

            public BrandEditCommandHandler(CozyDbContext db)
            {
                this.db = db;
            }

            public async Task<Brand> Handle(BrandEditCommand request, CancellationToken cancellationToken)
            {
                var entity = await db.Brands
                       .FirstOrDefaultAsync(bp => bp.Id == request.Id && bp.DeletedDate == null);
                if (entity == null)
                {
                    return null;
                }

                entity.Name = request.Name;

                await db.SaveChangesAsync(cancellationToken);

                return entity;
            }


        }
    }
}
