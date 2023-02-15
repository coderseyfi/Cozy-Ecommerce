using Cozy.Domain.AppCode.Infrastructure;
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
        public string Address { get; set; }

        [Required(ErrorMessage = "{0} cannot be left empty")]
        public string PhoneNumber { get; set; }

        public decimal TotalPrice { get; set; }
        public string Notes { get; set; }


    }
}
