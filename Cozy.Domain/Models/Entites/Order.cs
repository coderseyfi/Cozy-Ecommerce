using Cozy.Domain.AppCode.Infrastructure;
using Cozy.Domain.Models.Entities.Membership;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cozy.Domain.Models.Entites
{
    public class Order : BaseEntity
    {

        [Required(ErrorMessage = "{0} cannot be left empty")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "{0} cannot be left empty")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "{0} cannot be left empty")]
        public string PhoneNumber { get; set; }

        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "{0} cannot be left empty")]
        public string Address { get; set; }

        public virtual CozyUser User { get; set; }

        public int UserId { get; set; }

        public bool IsDelivered { get; set; } = false;

        public ICollection<OrderProduct> OrderProducts { get; set; }


    }
}
