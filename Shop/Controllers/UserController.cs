using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Cart()
        {
            if (User.Identity.IsAuthenticated == true)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var myCart = _context.Carts
                         .Include(x => x.Orders)
                         .ThenInclude(x => x.Product)
                         .SingleOrDefault(x => string.IsNullOrEmpty(x.Status) &&
                         x.UserId == user.Id);
                return View(myCart);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrder(string ids)
        {
            var id = ids.Trim().Split(' ');
            foreach (var item in id)
            {
                var order = _context.Orders.FirstOrDefault(x => x.Id.ToString() == item);
                _context.Entry(order).State = EntityState.Deleted;
            }
            _context.SaveChanges();

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var cart = _context.Carts
                   .SingleOrDefault(x => string.IsNullOrEmpty(x.Status) &&
                   x.UserId == user.Id);

            return Json(new
            {
                ids,
                totalPrices = cart.TotalPriceCurency()
            });
        }

        [HttpPost]
        public IActionResult SelectOrder(int id, bool val)
        {
            var order = _context.Orders.Find(id);
            order.Buy = val;
            _context.Entry(order).State = EntityState.Modified;
            _context.SaveChanges();

            var total = _context.Carts
                .Include(x => x.Orders)
                .ThenInclude(x => x.Product)
                .FirstOrDefault(x => x.Id == order.CartId);
            if (total == null)
                return Json("0 đ");
            return Json(total.TotalPriceCurency());
        }

        [HttpPost]
        public IActionResult ChangeQuantity(int id, int val)
        {
            var order = _context.Orders.Find(id);
            order.Quantity = val;
            _context.Entry(order).State = EntityState.Modified;
            _context.SaveChanges();

            var total = _context.Carts
                .Include(x => x.Orders)
                .ThenInclude(x => x.Product)
                .FirstOrDefault(x => x.Id == order.CartId);

            return Json(new
            {
                totalPrices = total.TotalPriceCurency(),
                totalPrice = order.TotalPriceCurency()
            });
        }
    }
}