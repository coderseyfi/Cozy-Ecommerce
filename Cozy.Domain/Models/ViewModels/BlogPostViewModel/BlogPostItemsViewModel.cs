using Cozy.Domain.Models.Entites;
using System.Collections.Generic;

namespace Cozy.Domain.Models.ViewModels.BlogPostItemsViewModel
{
    public class BlogPostItemsViewModel
    {
        public BlogPost BlogPost { get; set; }

        public ICollection<BlogPostLike> BlogPostLikes { get; set; }
    }
}
