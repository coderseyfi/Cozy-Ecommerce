using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Cozy.Domain.AppCode.Extensions;
using Cozy.Domain.AppCode.Infrastructure;
using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cozy.Domain.Business.BlogPostModule
{

    public class BlogPostEditCommand : IRequest<JsonResponse>
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        public string ImagePath { get; set; }

        public int? CategoryId { get; set; }

        public IFormFile Image { get; set; }

        public int[] TagIds { get; set; }

        public class BlogPostEditCommandHandler : IRequestHandler<BlogPostEditCommand, JsonResponse>
        {
            private readonly CozyDbContext db;
            private readonly IHostEnvironment env;

            public BlogPostEditCommandHandler(CozyDbContext db, IHostEnvironment env)
            {
                this.db = db;
                this.env = env;
            }

            public async Task<JsonResponse> Handle(BlogPostEditCommand request, CancellationToken cancellationToken)
            {
                var entity = await db.BlogPosts
                       .Include(bp => bp.TagCloud)
                       .FirstOrDefaultAsync(bp => bp.Id == request.Id && bp.DeletedDate == null);
                if (entity == null)
                {
                    return null;
                }

                entity.Body = request.Body;
                entity.Title = request.Title;
                entity.CategoryId = request.CategoryId;

                if (request.Image == null)
                    goto end;

                string extexsion = Path.GetExtension(request.Image.FileName); //.jpg, png 

                request.ImagePath = $"blogpost-{Guid.NewGuid().ToString().ToLower()}{extexsion}";

                string fullPath = env.GetImagePhysicalPath(request.ImagePath);


                using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    request.Image.CopyTo(fs);
                }

                string oldPath = env.GetImagePhysicalPath(entity.ImagePath);

              

                env.ArchiveImage(entity.ImagePath);

                entity.ImagePath = request.ImagePath;

            end:

                if (string.IsNullOrWhiteSpace(entity.Slug))
                {
                    entity.Slug = request.Title.ToSlug();
                }

                if (request.TagIds == null && entity.TagCloud.Any())
                {
                    foreach (var tagItem in entity.TagCloud)
                    {
                        db.BlogPostTagCloud.Remove(tagItem);
                    }
                }
                else if (request.TagIds != null)
                {
                    #region database de evvel olan indi almayan tagler - silinmesini istediklerimiz

                    // 1, 2, 3 => 1, 3  (ashagidaki kod 2 ni tapir)
                    //evvel olan indi olmayan
                    var exceptedIds = db.BlogPostTagCloud.Where(tc => tc.BlogPostId == entity.Id).Select(tc => tc.TagId).ToList()
                        .Except(request.TagIds).ToArray();

                    if (exceptedIds.Length > 0)
                    {
                        foreach (var exceptedId in exceptedIds)
                        {
                            var tagItem = await db.BlogPostTagCloud.FirstOrDefaultAsync(tc => tc.TagId == exceptedId
                            && tc.BlogPostId == entity.Id);

                            if (tagItem != null)
                            {
                                db.BlogPostTagCloud.Remove(tagItem);
                            }

                        }
                    }
                    #endregion


                    #region evvel database de olmayan amma indi olmasini istediklerimiz

                    // 1, 2, 3 => 1, 2, 3, 4  (ashagidaki kod 4 ni tapir)
                    //evvel olmayan indi olan
                    var newExceptedIds = request.TagIds.Except(db.BlogPostTagCloud.Where(tc => tc.BlogPostId == entity.Id).Select(tc => tc.TagId).ToList()).ToArray();

                    if (newExceptedIds.Length > 0)
                    {
                        foreach (var exceptedId in newExceptedIds)
                        {
                            var tagItem = new BlogPostTagItem();

                            tagItem.TagId = exceptedId;
                            tagItem.BlogPostId = entity.Id;

                            await db.BlogPostTagCloud.AddAsync(tagItem);
                        }
                    }
                    #endregion

                }



                await db.SaveChangesAsync(cancellationToken);

                return new JsonResponse
                {
                    Error = false,
                    Message = "Success"
                };
            }


        }
    }
}
