using Cozy.Domain.Business.BlogPostModule;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cozy.WebUI.AppCode.ViewComponents
{
    public class RecentPostsHomeViewComponent : ViewComponent
    {
        private readonly IMediator mediator;

        public RecentPostsHomeViewComponent(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var query = new ProductRecentArrival() { Size = 4 };
            var posts = await mediator.Send(query);

            return View(posts);
        }
    }
}
