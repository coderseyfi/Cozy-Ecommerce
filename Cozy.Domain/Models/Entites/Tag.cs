using Cozy.Domain.AppCode.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cozy.Domain.Models.Entites
{
    public class Tag : BaseEntity
    {
        [Required]
        public string Text { get; set; }

        public virtual ICollection<BlogPostTagItem> TagCloud { get; set; }
    }
}
