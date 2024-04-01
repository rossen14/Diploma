using Diploma.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System;
using Diploma.Data;
using System.Linq;

namespace Diploma.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext _context;
        public OrdersController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [Authorize]
        [HttpGet]
        public IActionResult All()
        {
            ViewBag.Products = _context.Products.ToList();
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult MyProducts()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Order[] GetMyOrders =
                this._context
                .Orders
                .Where(o => o.ApplicationUser.Id == userId)
                .Include(o => o.Product)
                .ToArray();
            return this.View(GetMyOrders);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult AllOrders()
        {
            var orders = _context.Orders
                .Include(x => x.Product)
                .Include(x => x.ApplicationUser)
                    .ToList();
            return this.View(orders);
        }

        [Authorize]
        [HttpPost]
        public IActionResult OrderProducts(Order model)
        {
            var userID = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            ApplicationUser user = _context.Users.FirstOrDefault(x => x.Id == userID);
            var _product = this._context.Products.Find(model.Product.ID);
            var order = new Order()
            {
                OrderedOn = DateTime.UtcNow,
                Product = _product,
                ApplicationUser = user,
                Quantity = model.Quantity
            };
            _product.Stock -= model.Quantity;
            _context.Orders.Add(order);
            _context.SaveChanges();
            return RedirectToAction("MyProducts", "Orders");
        }
    }
}
