using Cozy.Domain.AppCode.Infrastructure;
using Cozy.Domain.Models.Entities.Membership;
using System;

namespace Cozy.Domain.Models.Entites
{
    public class ProductRate
    {
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public byte Rate { get; set; }
        public int? CreatedByUserId { get; set; }
        public CozyUser CreatedByUser { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow.AddHours(4);
        public int? DeletedUserId { get; set; }
        public CozyUser DeletedByUser { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
