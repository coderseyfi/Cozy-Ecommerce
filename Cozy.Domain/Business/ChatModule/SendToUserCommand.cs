using Cozy.Domain.AppCode.Extensions;
using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites.Chat;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace Cozy.Domain.Business.ChatModule
{
    public class SendToUserCommand : IRequest<Message>
    {
        public HubCallerContext HubContext { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }

        public class SendToUserCommandHandler : IRequestHandler<SendToUserCommand, Message>
        {
            private readonly CozyDbContext db;

            public SendToUserCommandHandler(CozyDbContext db)
            {
                this.db = db;
            }
            public async Task<Message> Handle(SendToUserCommand request, CancellationToken cancellationToken)
            {
                var httpContext = request.HubContext.GetHttpContext();

                if (httpContext.User.Identity.IsAuthenticated)
                {
                    var message = new Message();

                    message.FromId = httpContext.User.GetCurrentUserId();
                    message.ToId = request.UserId;
                    message.Text = request.Message;

                    await db.Messages.AddAsync(message, cancellationToken);
                    await db.SaveChangesAsync(cancellationToken);
                    return message;
                }

                return null;
            }
        }
    }
}
