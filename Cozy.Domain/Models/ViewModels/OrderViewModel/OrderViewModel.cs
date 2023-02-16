using Cozy.Domain.Models.Entites;
using System.Collections.Generic;

namespace Cozy.Domain.Models.ViewModels.OrderViewModel
{
    public class OrderViewModel
    {
        public IEnumerable<Basket> BasketProducts { get; set; }

        public Order OrderDetails { get; set; }
    }
}
