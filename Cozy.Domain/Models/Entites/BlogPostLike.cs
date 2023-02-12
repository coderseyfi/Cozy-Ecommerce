using Cozy.Domain.AppCode.Infrastructure;
using Cozy.Domain.Models.Entities.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cozy.Domain.Models.Entites
{
    public class BlogPostLike : BaseEntity
    {
        public int BlogPostId { get; set; }

        public virtual BlogPost BlogPost { get; set; }

        public CozyUser CreatedByUser { get; set; }

        public int CreatedByUserId { get; set; }
    }
}
