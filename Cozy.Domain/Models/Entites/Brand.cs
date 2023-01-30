using Cozy.Domain.AppCode.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cozy.Domain.Models.Entites
{
   public class Brand : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? DeletedByUserId { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
