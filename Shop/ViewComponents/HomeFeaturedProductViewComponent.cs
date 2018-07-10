using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.ViewComponents
{
    public class HomeFeaturedProductViewComponent : ViewComponent
    {

        private readonly ApplicationDbContext _context;

        public HomeFeaturedProductViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {

            var models = _context.CategoryProducts.Include(x => x.Products)
                .OrderByDescending(x => x.CreatedDate).Take(10).ToList();
            return View(models);
        }
    }
}
