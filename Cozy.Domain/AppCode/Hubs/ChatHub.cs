using Cozy.Domain.AppCode.Extensions;
using Cozy.Domain.Business.ChatModule;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Cozy.Domain.AppCode.Hubs
{

    #region New Hub

    //public class ChatHub : Hub
    //{
    //    private readonly IMediator mediator;
    //    static ConcurrentDictionary<int, string> clients = new ConcurrentDictionary<int, string>();

    //    public ChatHub(IMediator mediator)
    //    {
    //        this.mediator = mediator;
    //    }

    //    public override async Task OnConnectedAsync()
    //    {
    //        await base.OnConnectedAsync();

    //        var httpContext = Context.GetHttpContext();
    //        if (httpContext.User.Identity.IsAuthenticated)
    //        {
    //            var userId = httpContext.User.GetCurrentUserId();

    //            clients.AddOrUpdate(userId, Context.ConnectionId, (k, v) => v);

    //            var command = new HubUserConnectCommand
    //            {
    //                HubContext = Context,
    //                HubGroups = Groups
    //            };

    //            await mediator.Send(command);

    //        }


    //    }

    //    public override Task OnDisconnectedAsync(Exception exception)
    //    {
    //        return base.OnDisconnectedAsync(exception);
    //    }


    //    //istifadeci gonderir mesaj operatorlara gedir
    //    public async Task SendMessage(string message)
    //    {
    //        var command = new SendToGroupCommand
    //        {
    //            HubContext = Context,
    //            GroupName = "CallCenter",
    //            Message = message
    //        };

    //        var response = await mediator.Send(command);

    //        var httpContext = Context.GetHttpContext();
    //        var userId = httpContext.User.GetCurrentUserId();

    //        //save to db

    //        await Clients.GroupExcept(command.GroupName, Context.ConnectionId)
    //            .SendAsync("ReceiveFromOperator", JsonConvert.SerializeObject(response));
    //    }


    //    //operator gonderir mesaj istifadecilere gedir
    //    public async Task SendFromOperator(string message, int toUserId)
    //    {
    //        var httpContext = Context.GetHttpContext();
    //        var userId = httpContext.User.GetCurrentUserId();

    //        var command = new SendToUserCommand
    //        {
    //            HubContext = Context,
    //            UserId = toUserId,
    //            Message = message
    //        };

    //        var response = await mediator.Send(command);

    //        if (response != null && clients.TryGetValue(response.ToId.Value, out string toUserConnectionId))
    //        {
    //            await Clients.User(toUserConnectionId)
    //                .SendAsync("ReceiveFromClient", response.ToId.Value, response.Text);
    //        }
    //    }
    //}
    #endregion

    public class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        

        //istifadeci gonderir mesaj operatorlara gedir
        public async Task SendMessage(string message)
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.User.GetCurrentUserId();

            //save to db
            await Clients.Group("CallCenter").SendAsync("ReceiveFromOperator", userId, message);

        }

        //operator gonderir istifadeciye gedir
        public async Task SendFromOperator(string message)
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.User.GetCurrentUserId();

            //save to db
            await Clients.All.SendAsync("ReceiveFromClient", userId, message);

        }
    }





}
