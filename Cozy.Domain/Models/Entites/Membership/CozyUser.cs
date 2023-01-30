using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Cozy.Domain.Models.Entities.Membership
{
    public class CozyUser : IdentityUser<int>
    {
        [Required]
        public string Name { get; set; }
        [Required]

        public string Surname { get; set; }
    }
}
