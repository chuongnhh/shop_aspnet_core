using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
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

        public CartViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {

            var models = _context.Carts
                .Include(x => x.Orders)
                .ThenInclude(x => x.Product)
                .SingleOrDefault(x => string.IsNullOrEmpty(x.Status));
            if (models != null)
            {
                var cartViewModels = new CartViewModels
                {
                    Id = models.Id,
                    ItemsCount = models.Orders.Count,
                    Total = models.Orders.Sum(x => x.GetTotal()),
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
    }
}
