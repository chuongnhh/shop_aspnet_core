using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.ViewComponents {
    public class HomeLastestProductViewComponent : ViewComponent {

        private readonly ApplicationDbContext _context;

        public HomeLastestProductViewComponent(ApplicationDbContext context) {
            _context = context;
        }

        public IViewComponentResult Invoke() {

            var models = _context.Products.OrderByDescending(x => x.CreatedDate).Take(6).ToList();
            return View(models);
        }
    }
}
