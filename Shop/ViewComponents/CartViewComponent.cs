using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shop.Data;
using Shop.Extensions;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {

        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;
        public CartViewComponent(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            if (User.Identity.IsAuthenticated)
            {
                var cart = _context.Carts
                    .Include(x => x.Orders)
                    .ThenInclude(x => x.Product)
                    .Where(x => x.UserId == _userManager.GetUserId(HttpContext.User))
                    .SingleOrDefault(x => string.IsNullOrEmpty(x.Status));

                if (cart != null)
                {
                    var cartViewModels = new CartViewModels
                    {
                        Id = cart.Id,
                        ItemsCount = cart.Orders.Count,
                        Total = cart.Orders.Sum(x => x.GetTotal()),
                        Status = null
                    };

                    return View(cartViewModels);
                }
                else
                {
                    var cartViewModels = new CartViewModels
                    {
                        Id = 0,
                        ItemsCount = 0,
                        Total = 0,
                        Status = "Cart is empty"
                    };

                    return View(cartViewModels);
                }
            }
            else
            {
                if (HttpContext.Session.GetString("SESSION_CART") == null || string.IsNullOrEmpty(HttpContext.Session.GetString("SESSION_CART")) == true)
                {

                    var cartViewModels = new CartViewModels
                    {
                        Id = 0,
                        ItemsCount = 0,
                        Total = 0,
                        Status = "Cart is empty"
                    };

                    return View(cartViewModels);
                }
                else
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
                    var cartViewModels = new CartViewModels
                    {
                        Id = cart.Id,
                        ItemsCount = cart.Orders.Count,
                        Total = cart.Orders.Sum(x => x.GetTotal()),
                        Status = null
                    };

                    return View(cartViewModels);
                }
            }
        }
    }
}
