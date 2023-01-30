using MediatR;
using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Cozy.Domain.Business.BrandModule
{
    public class BrandCreateCommand : IRequest<Brand>
    {
        [Required]
        public string Name { get; set; }


        public class BrandCreateCommandHandler : IRequestHandler<BrandCreateCommand, Brand>
        {
            private readonly CozyDbContext db;

            public BrandCreateCommandHandler(CozyDbContext db)
            {
                this.db = db;
            }

            public async Task<Brand> Handle(BrandCreateCommand request, CancellationToken cancellationToken)
            {
                var entity = new Brand();

                entity.Name = request.Name;

                await db.Brands.AddAsync(entity, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                return entity;
            }


        }
    }

}
