using Cozy.Domain.AppCode.Extensions;
using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites.Chat;
using Cozy.Domain.Models.ViewModels;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Cozy.Domain.Business.ChatModule
{
    public class SendToGroupCommand : IRequest<MessageViewModel>
    {
        public HubCallerContext HubContext { get; set; }
        public string GroupName { get; set; }
        public string Message { get; set; }

        public class SendToGroupCommandHandler : IRequestHandler<SendToGroupCommand, MessageViewModel>
        {
            private readonly CozyDbContext db;

            public SendToGroupCommandHandler(CozyDbContext db)
            {
                this.db = db;
            }
            public async Task<MessageViewModel> Handle(SendToGroupCommand request, CancellationToken cancellationToken)
            {

                var httpContext = request.HubContext.GetHttpContext();

                var group = await db.ChatGroups
                    .FirstOrDefaultAsync(cg => cg.Name.Equals(request.GroupName), cancellationToken);

                var message = new Message();

                message.FromId = httpContext.User.GetCurrentUserId();
                message.GroupId = group.Id;
                message.Text = request.Message;


                await db.Messages.AddAsync(message, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                message = await db.Messages
                     .Include(m => m.From)
                    .FirstOrDefaultAsync(m => m.Id == message.Id, cancellationToken);

                return new MessageViewModel
                {
                    FriendId = message.FromId,
                    FriendName = message.From.UserName,
                    LastMessage = message.Text,
                    Date = message.CreatedDate.ToString("HH:mm")
                };
            }
        }
    }
}
