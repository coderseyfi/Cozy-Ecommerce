using MediatR;
using Microsoft.AspNetCore.Http;
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
    public class BlogPostCreateCommand : IRequest<BlogPost>
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        public string ImagePath { get; set; }

        public int? CategoryId { get; set; }

        public IFormFile Image { get; set; }

        public int[] TagIds { get; set; }

        public class BlogPostCreateCommandHandler : IRequestHandler<BlogPostCreateCommand, BlogPost>
        {
            private readonly CozyDbContext db;
            private readonly IHostEnvironment env;

            public BlogPostCreateCommandHandler(CozyDbContext db, IHostEnvironment env)
            {
                this.db = db;
                this.env = env;
            }

            public async Task<BlogPost> Handle(BlogPostCreateCommand request, CancellationToken cancellationToken)
            {
                var entity = new BlogPost();
                entity.TagCloud = new List<BlogPostTagItem>();

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

                entity.ImagePath = request.ImagePath;

            end:

                entity.Slug = request.Title.ToSlug();

                if (request.TagIds != null)
                {
                    foreach (var exceptedId in request.TagIds)
                    {
                        var tagItem = new BlogPostTagItem();

                        tagItem.TagId = exceptedId;
                        entity.TagCloud.Add(tagItem);
                    }

                }


                await db.BlogPosts.AddAsync(entity, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                return entity;
            }


        }
    }

}
