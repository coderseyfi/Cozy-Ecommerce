using Cozy.Domain.Models.Entites;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cozy.Domain.AppCode.Extensions
{
    static public partial class Extension
    {
        static public string GetCategoriesRaw(this ICollection<Category> categories)
        {
            if (categories == null || !categories.Any())
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("<ul class='list-categories' data-entity-name='categories'>");

            foreach (var category in categories)
            {
                FillCategoriesRaw(category);
            }

            sb.Append("</ul>");

            return sb.ToString();

            void FillCategoriesRaw(Category category)
            {
                sb.Append($"<li class='list-category-item category-hover-item'>");

                if (category.ParentId != null)
                {
                    sb.Append($"<input type='checkbox' id='cb-{category.Id}' />");
                }

                sb.Append($"<label for='cb-{category.Id}' class='label-text-category label-text-category-{category.Id}' data-entity-id='{category.Id}' >{category.Name}</label>");

                if (category.Children != null && category.Children.Any())
                {
                    sb.Append($"<i class='fa fa-chevron-down show-children show-children-{category.Id}' data-btn-id='{category.Id}'></i>");
                    sb.Append($"<ul class='children-categories ul-{category.Id}' style='display: none;' data-id='{category.Id}'>");

                    foreach (var child in category.Children)
                    {
                        FillCategoriesRaw(child);
                    }

                    sb.Append("</ul>");
                }

                sb.Append("</li>");
            }
        }


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