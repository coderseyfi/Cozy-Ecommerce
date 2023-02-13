using Cozy.Domain.Models.Entites;

namespace Cozy.Domain.Models.ViewModels
{

    public class SearchPanelViewModel
    {
        public ProductColor[] Colors { get; set; }
        public ProductMaterial[] Materials { get; set; }
        public Brand[] Brands { get; set; }
        public Category[] Categories { get; set; }

        public int Min { get; set; }
        public int Max { get; set; }
    }
}
