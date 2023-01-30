using Cozy.Domain.AppCode.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Cozy.Domain.Models.Entites
{
    public class Faq : BaseEntity
    {
        [Required]
        public string Question { get; set; }

        [Required]
        public string Answer { get; set; }
    }
}
