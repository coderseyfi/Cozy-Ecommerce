using Cozy.Domain.AppCode.Extensions;
using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cozy.Domain.Business.BlogPostModule
{
    public class BlogPostCommentCommand : IRequest<BlogPostComment>
    {
        public int? CommentId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Post Id uygun deyil")]
        public int PostId { get; set; }

        [Required]
        public string Comment { get; set; }

        public class BlogPostCommentCommandHandler : IRequestHandler<BlogPostCommentCommand, BlogPostComment>
        {

            private readonly CozyDbContext db;
            private readonly IActionContextAccessor ctx;

            public BlogPostCommentCommandHandler(CozyDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }

            public async Task<BlogPostComment> Handle(BlogPostCommentCommand request, CancellationToken cancellationToken)
            {
                if (!ctx.ActionContext.ModelState.IsValid)
                {
                    throw new Exception(ctx.ActionContext.ModelState.GetError().FirstOrDefault().Message);
                }

                var post = await db.BlogPosts.FirstOrDefaultAsync(bp => bp.Id == request.PostId);

                if (post == null)
                {
                    throw new Exception("Post movcud deyil");
                }

               

                var commentModel = new BlogPostComment
                {
                    BlogPostId = request.PostId,
                    Text = request.Comment,
                    CreatedByUserId = ctx.GetCurrentUserId()
                };



                if (request.CommentId.HasValue && await db.BlogPostComments.AnyAsync(c => c.Id == request.CommentId))
                {
                    commentModel.ParentId = request.CommentId;
                }

                db.BlogPostComments.Add(commentModel);
                await db.SaveChangesAsync();

                commentModel = await db.BlogPostComments
                    .Include(c => c.CreatedByUser)
                    .Include(c => c.Parent)
                    .FirstOrDefaultAsync(c => c.Id == commentModel.Id);

                return commentModel;
            }
        }
    }
}