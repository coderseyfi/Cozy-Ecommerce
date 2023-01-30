using Cozy.Domain.AppCode.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Cozy.Domain.Models.Entites
{
    public class ContactInfo : BaseEntity
    {
        [Required]
        public string PhoneNumber { get; set; }

        public string Location { get; set; }

        public string EmailAddress { get; set; }
    }
}
