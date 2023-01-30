using Cozy.Domain.AppCode.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace Cozy.Domain.Models.Entites
{
    public class Subscribe:BaseEntity
    {

        [Required(ErrorMessage = "{0} cannot be left empty")]
        public string Email { get; set; }
        public bool IsApproved { get; set; } = false;
        public DateTime? ApprovedDate { get; set; }
    }
}
