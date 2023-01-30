using Cozy.Domain.AppCode.Infrastructure;
using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cozy.Domain.Business.BlogPostModule
{
    public class BlogPostGetAllQueryAdmin : PaginateModel, IRequest<PagedViewModel<BlogPost>>
    {

        public class BlogPostGetAllQueryAdminHandler : IRequestHandler<BlogPostGetAllQueryAdmin, PagedViewModel<BlogPost>>
        {
            private readonly CozyDbContext db;

            public BlogPostGetAllQueryAdminHandler(CozyDbContext db)
            {
                this.db = db;
            }
            public async Task<PagedViewModel<BlogPost>> Handle(BlogPostGetAllQueryAdmin request, CancellationToken cancellationToken)
            {
                //var posts = await db.BlogPosts
                //.Where(bp => bp.DeletedDate == null && bp.PublishedDate != null)
                //.ToListAsync(cancellationToken);

                int skipSize = (request.PageIndex - 1) * request.PageSize;

                var query = db.BlogPosts
                .Where(bp => bp.DeletedDate == null)
                .OrderByDescending(bp => bp.PublishedDate)
                .AsQueryable();

                var pagedModel = new PagedViewModel<BlogPost>(query, request);

                return pagedModel;
            }
        }
    }
}
