using Cozy.Domain.Business.ProductModule;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cozy.Domain.Models.ViewComponents
{
    public class BasketInfoNavViewComponent : ViewComponent
    {
        private readonly IMediator mediator;

        public BasketInfoNavViewComponent(IMediator mediator)
        {
            this.mediator = mediator;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var query = new ProductBasketQuery();

            var response = await mediator.Send(query);

            return View(response);
        }
    }
}