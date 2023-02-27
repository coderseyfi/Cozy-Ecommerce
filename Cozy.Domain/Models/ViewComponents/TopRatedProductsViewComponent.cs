using Cozy.Domain.Business.BlogPostModule;
using Cozy.Domain.Business.ProductModule;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cozy.WebUI.AppCode.ViewComponents
{
    public class TopRatedProductsViewComponent : ViewComponent
    {
        private readonly IMediator mediator;

        public TopRatedProductsViewComponent(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var query = new TopRatedProductQuery();

            var posts = await mediator.Send(query);

            return View(posts);
        }
    }
}
