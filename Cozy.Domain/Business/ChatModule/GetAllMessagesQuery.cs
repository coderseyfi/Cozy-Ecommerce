using MediatR;
using Microsoft.EntityFrameworkCore;
using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cozy.Domain.Models.Entites.Chat;

namespace Cozy.Domain.Business.CategoryModule
{
    public class GetAllMessagesQuery : IRequest<List<Message>>
    {
        public class GetAllMessagesQueryHandler : IRequestHandler<GetAllMessagesQuery, List<Message>>
        {
            private readonly CozyDbContext db;

            public GetAllMessagesQueryHandler(CozyDbContext db)
            {
                this.db = db;
            }
            public async Task<List<Message>> Handle(GetAllMessagesQuery request, CancellationToken cancellationToken)
            {
                var data = await db.Messages
                .Where(bp => bp.DeletedDate == null)
                .ToListAsync(cancellationToken);

                return data;
            }
        }


    }
}
