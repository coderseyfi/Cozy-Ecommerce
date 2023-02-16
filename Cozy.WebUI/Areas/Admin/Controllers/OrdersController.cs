using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cozy.Domain.Business.OrderModule;
using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entites;

namespace Cozy.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly CozyDbContext db;
        private readonly IMediator mediator;

        public OrdersController(CozyDbContext db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }

        [Authorize(Policy = "admin.orders.index")]
        public async Task<IActionResult> Index(OrderGetAllQuery query)
        {
            var response = await mediator.Send(query);

            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }

        [Authorize(Policy = "admin.orders.details")]
        public async Task<IActionResult> Details(OrderGetSingleQuery query)
        {
            var response = await mediator.Send(query);

            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.orders.delete")]
        public async Task<IActionResult> DeleteConfirmed(OrderRemoveCommand command)
        {
            var response = await mediator.Send(command);

            if (response == null)
            {
                return NotFound();
            }


            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.orders.completeorder")]
        public async Task<IActionResult> CompleteOrder(OrderCompleteCommand command)
        {
            var response = await mediator.Send(command);

            if (response == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "admin.orders.deliveredorders")]
        public async Task<IActionResult> DeliveredOrders(OrderGetAllDeliveredQuery query)
        {
            var response = await mediator.Send(query);
            return View(response);
        }

        [Authorize(Policy = "admin.orders.cancelledorders")]
        public async Task<IActionResult> CancelledOrders(OrderGetAllCancelledQuery query)
        {
            var response = await mediator.Send(query);
            return View(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.orders.cancelledordersbacktoindex")]
        public async Task<IActionResult> CancelledOrdersBackToIndex(CancelledOrderRemoveBackCommand command)
        {
            var response = await mediator.Send(command);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.orders.deliveredordersbacktoindex")]
        public async Task<IActionResult> DeliveredOrdersBackToIndex(DeliveredOrderRemoveBackCommand command)
        {
            var response = await mediator.Send(command);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Clear")]
        [Authorize(Policy = "admin.orders.clearcancelledorders")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClearCancelledOrders(int id, OrderClearCommand command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            var response = await mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Any(e => e.Id == id);
        }
    }
}
