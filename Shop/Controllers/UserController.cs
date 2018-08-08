using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
                if (HttpContext.Session.GetString("SESSION_CART") != null && string.IsNullOrEmpty(HttpContext.Session.GetString("SESSION_CART")) == false)
                {
                    var cart = JsonConvert.DeserializeObject<Cart>(HttpContext.Session.GetString("SESSION_CART"));
                    foreach (var item in cart.Orders)
                    {
                        var p = _context.Products.SingleOrDefault(x => x.Id == item.ProductId);
                        if (p != null)
                        {
                            item.Product = p;
                        }
                    }
                    return View(cart);
                }
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrder(string ids)
        {
            if (User.Identity.IsAuthenticated == true)
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
            else
            {
                var cart = JsonConvert.DeserializeObject<Cart>(HttpContext.Session.GetString("SESSION_CART"));
                var id = ids.Trim().Split(' ');
                foreach (var item in id)
                {
                    var order = cart.Orders.FirstOrDefault(x => x.Id.ToString() == item);
                    cart.Orders.Remove(order);
                }
                HttpContext.Session.SetString("SESSION_CART", JsonConvert.SerializeObject(cart));

                foreach (var item in cart.Orders)
                {
                    var p = _context.Products.SingleOrDefault(x => x.Id == item.ProductId);
                    if (p != null)
                    {
                        item.Product = p;
                    }
                }

                return Json(cart.TotalPriceCurency());
            }
        }

        [HttpPost]
        public IActionResult SelectOrder(int id, bool val)
        {
            if (User.Identity.IsAuthenticated == true)
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
            else
            {
                var cart = JsonConvert.DeserializeObject<Cart>(HttpContext.Session.GetString("SESSION_CART"));
                var order = cart.Orders.SingleOrDefault(x => x.Id == id);
                order.Buy = val;
                HttpContext.Session.SetString("SESSION_CART", JsonConvert.SerializeObject(cart));

                cart = JsonConvert.DeserializeObject<Cart>(HttpContext.Session.GetString("SESSION_CART"));
                foreach (var item in cart.Orders)
                {
                    var p = _context.Products.SingleOrDefault(x => x.Id == item.ProductId);
                    if (p != null)
                    {
                        item.Product = p;
                    }
                }

                return Json(cart.TotalPriceCurency());
            }
        }

        [HttpPost]
        public IActionResult ChangeQuantity(int id, int val)
        {
            if (User.Identity.IsAuthenticated == true)
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
            else
            {
                var cart = JsonConvert.DeserializeObject<Cart>(HttpContext.Session.GetString("SESSION_CART"));
                var order = cart.Orders.SingleOrDefault(x => x.Id == id);
                order.Quantity = val;
                HttpContext.Session.SetString("SESSION_CART", JsonConvert.SerializeObject(cart));

                cart = JsonConvert.DeserializeObject<Cart>(HttpContext.Session.GetString("SESSION_CART"));
                foreach (var item in cart?.Orders)
                {
                    var pr = _context.Products.SingleOrDefault(x => x.Id == item.ProductId);
                    if (pr != null)
                    {
                        item.Product = pr;
                    }
                }

                var p = _context.Products.SingleOrDefault(x => x.Id == order.ProductId);
                if (p != null)
                {
                    order.Product = p;
                }

                return Json(new
                {
                    totalPrices = cart.TotalPriceCurency(),
                    totalPrice = order.TotalPriceCurency()
                });
            }
        }


        public async Task<IActionResult> Payment()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var myCart = _context.Carts
                     .Include(x => x.Orders)
                     .ThenInclude(x => x.Product)
                     .SingleOrDefault(x => string.IsNullOrEmpty(x.Status) &&
                     x.UserId == user.Id);
            return View(myCart);
        }

        [HttpPost]
        public async Task<IActionResult> Payment(Cart cart)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var myCart = _context.Carts.SingleOrDefault(x => x.Id == cart.Id);
            myCart.Status = "Comfirmed";
            myCart.CustomerName = cart.CustomerName;
            myCart.CustomerEmail = cart.CustomerEmail;
            myCart.CustomerPhoneNumber = cart.CustomerPhoneNumber;
            myCart.CustomerAddress = cart.CustomerAddress;
            _context.Entry(myCart).State = EntityState.Modified;

            _context.SaveChanges();

            return RedirectToAction("Index","Home",null);
        }
    }
}