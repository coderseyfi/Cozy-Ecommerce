using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cozy.Domain.AppCode.Behaviors
{
    ////MyQuery - TRequest
    ////bool   - TResponse
    //public class MyQuery : IRequest<bool>
    //{

    //}
    public class LogBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> logger;

        public LogBehavior(ILogger<TRequest> logger)
        {
            this.logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var configuration = new JsonSerializerSettings();
            configuration.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            string requestJson = JsonConvert.SerializeObject(request, configuration);

            logger.LogWarning($">> {requestJson}");
            var response = await next();
            string responseJson = JsonConvert.SerializeObject(response, configuration);
            logger.LogWarning($">> {responseJson}");

            return response;
        }
    }
}
