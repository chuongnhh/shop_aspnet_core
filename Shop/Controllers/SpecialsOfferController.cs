using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Data;
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
                // User logined
                var cart = _context.Carts
                    .SingleOrDefault(x => string.IsNullOrEmpty(x.Status));
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (cart == null)
                {
                    // Create cart
                    cart = new Cart
                    {
                        CreatedDate = DateTime.Now,
                        CustomerName = user.UserName,
                        CustomerEmail = user.Email,
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
            }
            else
            {
                // User not logined
            }
            return Json(new { success = 1 });
        }
    }
}