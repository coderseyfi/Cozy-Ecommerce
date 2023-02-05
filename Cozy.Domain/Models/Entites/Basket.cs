using Cozy.Domain.Models.Entities.Membership;

namespace Cozy.Domain.Models.Entites
{
    public class Basket
    {
        public int UserId { get; set; }
        public virtual CozyUser User { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
