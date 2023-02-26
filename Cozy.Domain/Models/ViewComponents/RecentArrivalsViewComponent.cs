using Cozy.Domain.Business.BlogPostModule;
using Cozy.Domain.Business.ProductModule;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cozy.WebUI.AppCode.ViewComponents
{
    public class RecentArrivalsViewComponent : ViewComponent
    {
        private readonly IMediator mediator;

        public RecentArrivalsViewComponent(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var query = new ProductRecentArrivalQuery() { Size = 4 };
            var posts = await mediator.Send(query);

            return View(posts);
        }
    }
}
