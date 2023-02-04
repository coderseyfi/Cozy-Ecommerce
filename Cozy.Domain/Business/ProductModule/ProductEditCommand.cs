using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites;
using Cozy.Domain.AppCode.Extensions;
using MediatR;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System;
using Microsoft.EntityFrameworkCore;

namespace Cozy.Domain.Business.ProductModule
{
    public class ProductEditCommand : IRequest<Product>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string StockKeepingUnit { get; set; }

        public decimal Price { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public int BrandId { get; set; }

        public int? CategoryId { get; set; }
        public ImageItem[] Images { get; set; }


        public class ProductEditCommandHandler : IRequestHandler<ProductEditCommand, Product>
        {
            private readonly CozyDbContext db;
            private readonly IHostEnvironment env;

            public ProductEditCommandHandler(CozyDbContext db, IHostEnvironment env)
            {
                this.db = db;
                this.env = env;
            }

            public async Task<Product> Handle(ProductEditCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await db.Products.FirstOrDefaultAsync(p =>
                     p.Id == request.Id
                     && p.DeletedDate == null
                    , cancellationToken);

                    if (model == null)
                    {
                        return null;
                    }

                    model.Name = request.Name;
                    model.StockKeepingUnit = request.StockKeepingUnit;
                    model.Price = request.Price;
                    model.ShortDescription = request.ShortDescription;
                    model.Description = request.Description;
                    model.BrandId = request.BrandId;
                    model.CategoryId = request.CategoryId;

                    /*
                     sekil deyisdirmek istese
                    elave sekil artirsa
                    var olan sekli silibse
                     */

                    if (request.Images != null && request.Images.Where(i => i.File != null).Count() > 0)
                    {
                        model.ProductImages = new List<ProductImage>();




                        foreach (var item in request.Images.Where(i => i.File != null))
                        {
                            var image = new ProductImage();
                            image.IsMain = item.IsMain;

                            string extension = Path.GetExtension(item.File.FileName);//.jpg

                            image.Name = $"product-{Guid.NewGuid().ToString().ToLower()}{extension}";

                            string fullName = env.GetImagePhysicalPath(image.Name);

                            using (var fs = new FileStream(fullName, FileMode.Create, FileAccess.Write))
                            {
                                await item.File.CopyToAsync(fs, cancellationToken);
                            }

                            model.ProductImages.Add(image);
                        }
                    }

                    await db.Products.AddAsync(model, cancellationToken);
                    await db.SaveChangesAsync(cancellationToken);


                    return model;

                }
                catch (System.Exception)
                {
                    return null;
                }

            }
        }
    }
}
