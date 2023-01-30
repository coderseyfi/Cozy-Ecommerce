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
    public class BlogPostComment : BaseEntity
    {

        [Required]
        public string Text { get; set; }

        public int BlogPostId { get; set; }

        public int? ParentId { get; set; }

        public virtual BlogPost BlogPost { get; set; }

        public virtual BlogPostComment Parent { get; set; }

        public CozyUser CreatedByUser { get; set; }

        public int CreatedByUserId { get; set; }

        public virtual ICollection<BlogPostComment> Children { get; set; }
    }
}
