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
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.ComponentModel.DataAnnotations;
using Cozy.Domain.Migrations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Cozy.Domain.Business.ProductModule
{
    public class ProductEditCommand : IRequest<Product>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]

        public string StockKeepingUnit { get; set; }
        [Required]

        public decimal Price { get; set; }
        [Required]

        public string ShortDescription { get; set; }
        [Required]

        public string Description { get; set; }

        public int BrandId { get; set; }

        public int CategoryId { get; set; }
        [Required]

        public int ColorId { get; set; }
        [Required]

        public int MaterialId { get; set; }
        [Required]

        public ImageItem[] Images { get; set; }



        public class ProductEditCommandHandler : IRequestHandler<ProductEditCommand, Product>
        {
            private readonly CozyDbContext db;
            private readonly IHostEnvironment env;
            private readonly IActionContextAccessor ctx;

            public ProductEditCommandHandler(CozyDbContext db, IHostEnvironment env, IActionContextAccessor ctx)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
            }

            public async Task<Product> Handle(ProductEditCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await db.Products
                     .Include(p => p.ProductImages.Where(i => i.DeletedByUserId == null))
                     .FirstOrDefaultAsync(p =>
                     p.Id == request.Id
                     && p.DeletedDate == null
                    , cancellationToken);
                    var data = await db.ProductCatalogItems.FirstOrDefaultAsync(p => p.ProductId == request.Id && p.DeletedDate == null, cancellationToken);
                    if (request.ColorId != data.ColorId || request.MaterialId != data.MaterialId)
                    {
                        db.ProductCatalogItems.Remove(data);
                        await db.SaveChangesAsync();

                        var Item = new ProductCatalogItem();
                        Item.ProductId = request.Id;
                        Item.ColorId = request.ColorId;
                        Item.MaterialId = request.MaterialId;
                        await db.ProductCatalogItems.AddAsync(Item);
                    }
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
                    1. sekil deyisdirmek istemirse
                    2. elave sekil artirsa + 
                    3 var olan sekli silibse
                     */

                    if (request.Images != null && request.Images.Count() > 0)
                    {


                        #region 2.Teze fayllar var
                        foreach (var imageItem in request.Images.Where(i => i.File != null && i.Id == null))
                        {

                            var image = new ProductImage();
                            image.IsMain = imageItem.IsMain;
                            image.ProductId = model.Id;

                            string extension = Path.GetExtension(imageItem.File.FileName);//.jpg

                            image.Name = $"product-{Guid.NewGuid().ToString().ToLower()}{extension}";

                            string fullName = env.GetImagePhysicalPath(image.Name);

                            using (var fs = new FileStream(fullName, FileMode.Create, FileAccess.Write))
                            {
                                await imageItem.File.CopyToAsync(fs, cancellationToken);
                            }

                            model.ProductImages.Add(image);
                        }
                        #endregion

                        #region 3.Movcud sekillerden silinibse
                        foreach (var item in request.Images.Where(i => i.Id > 0 && i.TempPath == null))
                        {
                            var productImage = await db.ProductImages.FirstOrDefaultAsync(pi => pi.Id == item.Id && pi.ProductId == model.Id && pi.DeletedByUserId == null);

                            if (productImage != null)
                            {
                                productImage.IsMain = false;
                                productImage.DeletedDate = DateTime.UtcNow.AddHours(4);
                                productImage.DeletedByUserId = ctx.GetCurrentUserId();
                            }
                        }
                        #endregion


                        #region 1.Movcud deyishdirmek istemese

                        foreach (var item in model.ProductImages)
                        {
                            var fromForm = request.Images.FirstOrDefault(i => i.Id == item.Id);

                            if (fromForm != null)
                            {
                                item.IsMain = fromForm.IsMain;
                            }


                        }
                        #endregion
                    }
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
