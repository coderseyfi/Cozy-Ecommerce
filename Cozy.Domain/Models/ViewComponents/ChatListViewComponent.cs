using Cozy.Domain.Business.CategoryModule;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cozy.WebUI.AppCode.ViewComponents
{
    public class ChatListViewComponent : ViewComponent
    {
        private readonly IMediator mediator;

        public ChatListViewComponent(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var query = new GetAllMessagesQuery();
            var posts = await mediator.Send(query);

            return View(posts);
        }
    }
}
