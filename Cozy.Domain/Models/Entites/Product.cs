using Cozy.Domain.AppCode.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cozy.Domain.Models.Entites
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public string StockKeepingUnit { get; set; }

        public decimal Price { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public int BrandId { get; set; }

        public virtual Brand Brand { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<ProductImage> ProductImages { get; set; }

        //public virtual ıcollection<productcatalogıtem> productcatalog { get; set; }
    }
}
