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
using Shop.Extensions;
using Shop.Models;

namespace Shop.Controllers
{
    public class SpecialsOfferController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SpecialsOfferController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int id)
        {
            //var model = _context.Products.SingleOrDefault(x => x.Id == id);
            var model = _context.Products.Find(id);

            var relateProducts = _context.Products
                .Where(x => x.CategoryProductId == model.CategoryProductId && x.Id != id).ToList();

            ViewBag.RelateProducts = relateProducts;

            return View(model);
        }

        public async Task<IActionResult> AddToCart(OrderViewModels model)
        {
            if (User.Identity.IsAuthenticated == true)
            {

                var user = await _userManager.GetUserAsync(HttpContext.User);
                // User logined
                var cart = _context.Carts
                    .SingleOrDefault(x => string.IsNullOrEmpty(x.Status) &&
                    x.UserId == user.Id);

                if (cart == null)
                {
                    // Create cart
                    cart = new Cart
                    {
                        CreatedDate = DateTime.Now,
                        CustomerName = user.UserName,
                        CustomerEmail = user.Email,
                        UserId = user.Id,
                        Total = 0,
                        Status = null,
                        CustomerPhoneNumber = user.PhoneNumber
                    };
                    _context.Entry(cart).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                }
                // Create order
                var order = new Order
                {
                    UserId = user.Id,
                    ProductId = model.Id,
                    CartId = cart.Id,
                    Quantity = model.Quantity,
                    Buy = false
                };
                _context.Entry(order).State = Microsoft.EntityFrameworkCore.EntityState.Added;

                await _context.SaveChangesAsync();
                var models = _context.Carts
                .Include(x => x.Orders)
                .ThenInclude(x => x.Product)
                .SingleOrDefault(x => string.IsNullOrEmpty(x.Status) &&
                x.UserId == user.Id);
                if (models != null)
                {
                    var cartViewModels = new CartViewModels
                    {
                        Id = models.Id,
                        ItemsCount = models.Orders.Count,
                        Total = models.Orders.Sum(x => x.GetTotal()),
                        Status = null
                    };

                    return Json(new
                    {
                        success = 1,
                        itemCount = cartViewModels.ItemsCount,
                        total = cartViewModels.TotalCurency()
                    });
                }
            }
            else
            {
                if (HttpContext.Session.GetString("SESSION_CART") == null || string.IsNullOrEmpty(HttpContext.Session.GetString("SESSION_CART")) == true)
                {
                    // Create cart
                    var cart = new Cart
                    {
                        CreatedDate = DateTime.Now,
                        Total = 0,
                        Status = null,
                    };
                    // Create order
                    var order = new Order
                    {
                        Id = 1,
                        ProductId = model.Id,
                        CartId = cart.Id,
                        Quantity = model.Quantity,
                        Buy = false
                    };
                    cart.Orders.Add(order);
                    HttpContext.Session.SetString("SESSION_CART", JsonConvert.SerializeObject(cart));
                }
                else
                {
                    var cart = JsonConvert.DeserializeObject<Cart>(HttpContext.Session.GetString("SESSION_CART"));
                    // Create order
                    var order = new Order
                    {
                        Id = cart.Orders.Count() + 1,
                        ProductId = model.Id,
                        CartId = cart.Id,
                        Quantity = model.Quantity,
                        Buy = false
                    };
                    cart.Orders.Add(order);
                    HttpContext.Session.SetString("SESSION_CART", JsonConvert.SerializeObject(cart));
                }


                var models = JsonConvert.DeserializeObject<Cart>(HttpContext.Session.GetString("SESSION_CART"));
                foreach (var item in models.Orders)
                {
                    var p = _context.Products.SingleOrDefault(x => x.Id == item.ProductId);
                    if (p != null)
                    {
                        item.Product = p;
                    }
                }
                if (models != null)
                {
                    var cartViewModels = new CartViewModels
                    {
                        Id = models.Id,
                        ItemsCount = models.Orders.Count,
                        Total = models.Orders.Sum(x => x.GetTotal()),
                        Status = null
                    };

                    return Json(new
                    {
                        success = 1,
                        itemCount = cartViewModels.ItemsCount,
                        total = cartViewModels.TotalCurency()
                    });
                }
            }


            return Json(new { success = 0, message = "Error" });
        }
    }
}