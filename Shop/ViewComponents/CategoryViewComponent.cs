using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.ViewComponents {
    public class CategoryViewComponent : ViewComponent {

        private readonly ApplicationDbContext _context;

        public CategoryViewComponent(ApplicationDbContext context) {
            _context = context;
        }

        public IViewComponentResult Invoke() {

            var models = _context.GroupProducts
                .Include(x => x.CategoryProducts)
                .ThenInclude(x => x.Products).ToList();
            return View(models);
        }
    }
}
