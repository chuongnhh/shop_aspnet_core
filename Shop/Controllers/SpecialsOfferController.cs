using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Data;

namespace Shop.Controllers
{
    public class SpecialsOfferController : Controller
    {

        private readonly ApplicationDbContext _context;

        public SpecialsOfferController(ApplicationDbContext context)
        {
            _context = context;
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
    }
}