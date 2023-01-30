using Cozy.Domain.Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cozy.Domain.AppCode.Extensions
{
    static public partial class CategoryExtension
    {
        static public IEnumerable<Category> GetAllChildren(this Category category)
        {
            if (category.ParentId != null)
            {
                yield return category;
            }


            if (category.Children != null)
            {
                foreach (var item in category.Children.SelectMany(c => c.GetAllChildren()))
                {
                    yield return item;
                }
            }


        }
    }
}
