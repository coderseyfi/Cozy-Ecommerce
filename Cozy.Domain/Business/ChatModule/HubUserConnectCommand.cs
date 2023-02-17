using Cozy.Domain.AppCode.Extensions;
using Cozy.Domain.Models.DataContexts;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cozy.Domain.Business.ChatModule
{
    public class HubUserConnectCommand : IRequest<bool>
    {
        public HubCallerContext HubContext { get; set; }
        public IGroupManager HubGroups { get; set; }


        public class HubUserConnectCommandHandler : IRequestHandler<HubUserConnectCommand, bool>
        {
            private readonly CozyDbContext db;
            private readonly IActionContextAccessor ctx;

            public HubUserConnectCommandHandler(CozyDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }

            public async Task<bool> Handle(HubUserConnectCommand request, CancellationToken cancellationToken)
            {
                var httpContext = request.HubContext.GetHttpContext();
                if (!httpContext.User.Identity.IsAuthenticated)
                    return false;

                var userConnectionId = request.HubContext.ConnectionId;

                var userId = httpContext.User.GetCurrentUserId();

                var userGroups = await db.UserGroups
                    .Include(ug => ug.Group)
                    .Where(ug=>ug.UserId == userId)
                    .ToListAsync(cancellationToken);

                foreach (var item in userGroups)
                {
                    await request.HubGroups.AddToGroupAsync(userConnectionId, item.Group.Name, cancellationToken);
                }

                return true;
            }
        }
    }
}
