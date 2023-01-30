using Cozy.Domain.AppCode.Infrastructure;
using System.Collections.Generic;

namespace Cozy.Domain.Models.Entites
{
    public class ProductColor : BaseEntity
    {
        public string HexCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? DeletedByUserId { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
