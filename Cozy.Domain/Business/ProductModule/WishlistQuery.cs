using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Cozy.Domain.Business.ProductModule
{
    public class WishlistQuery : IRequest<IEnumerable<Product>>
    {

        public class WishlistQueryHandler : IRequestHandler<WishlistQuery, IEnumerable<Product>>
        {
            private readonly CozyDbContext db;
            private readonly IActionContextAccessor ctx;

            public WishlistQueryHandler(CozyDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }

            public async Task<IEnumerable<Product>> Handle(WishlistQuery request, CancellationToken cancellationToken)
            {
                var favorites = ctx.ActionContext.HttpContext.Request.Cookies["favorites"]?
                  .Split(",", StringSplitOptions.RemoveEmptyEntries)
                  .Where(x => Regex.IsMatch(x, @"\d+"))
                  .Select(x => int.Parse(x))
                  .ToArray();

                if (favorites == null || favorites.Length == 0)
                {
                    return Enumerable.Empty<Product>();
                }

                var favs = await db.Products
                   .Include(p => p.ProductImages).Where(i => i.DeletedDate == null)
                   .Where(p => favorites.Contains(p.Id) && p.DeletedDate == null)
                   .ToListAsync();

                return favs;
            }


        }
    }
}