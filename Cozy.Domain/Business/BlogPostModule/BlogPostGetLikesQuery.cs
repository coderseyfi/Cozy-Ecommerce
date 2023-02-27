using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cozy.Domain.Business.BlogPostModule
{
    public class BlogPostGetLikesQuery : IRequest<List<BlogPostLike>>
    {
        public int BlogPostId { get; set; }

        public class BlogPostGetLikesQueryHandler : IRequestHandler<BlogPostGetLikesQuery, List<BlogPostLike>>
        {
            private readonly CozyDbContext db;

            public BlogPostGetLikesQueryHandler(CozyDbContext db)
            {
                this.db = db;
            }

            public async Task<List<BlogPostLike>> Handle(BlogPostGetLikesQuery request, CancellationToken cancellationToken)
            {
                var data = await db.BlogPostLikes
                    .Include(bpc => bpc.BlogPost)
                    .Include(bpc => bpc.CreatedByUser)
                    .Where(bpc => bpc.DeletedDate == null && bpc.BlogPostId == request.BlogPostId)
                    .ToListAsync();

                return data;
            }
        }
    }
}