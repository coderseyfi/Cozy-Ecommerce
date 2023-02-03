using Cozy.Domain.AppCode.Infrastructure;
using System.Collections.Generic;

namespace Cozy.Domain.Models.Entites
{
    public class ProductMaterial: BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<ProductCatalogItem> ProductCatalog { get; set; }
    }
}
