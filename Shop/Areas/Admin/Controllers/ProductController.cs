using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop.Data;

namespace Shop.Areas.Admin.Controllers {
    [Area("Admin")]
    public class ProductController : Controller {

        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context) {
            _context = context;
        }

        // GET: Admin/Product
        public async Task<IActionResult> Index() {
            return View(await _context.Products.Include(x => x.CategoryProduct).ToListAsync());
        }


        // GET: Admin/Product/Edit/5
        public async Task<IActionResult> Details(int? id) {

            var model = await _context.Products.SingleOrDefaultAsync(m => m.Id == id);

            if (model == null) {
                model = new Product();
                ViewBag.Categories = new SelectList(_context.CategoryProducts.ToList(), "Id", "Name");
                return View(model);
            }
            ViewBag.Categories = new SelectList(_context.CategoryProducts.ToList(), "Id", "Name", model.CategoryProductId);
            return View(model);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int id, [Bind("Id,Name,CategoryProductId,Price,Quantity,Display,Description,ImageUrl")] Product model) {

            if (ModelState.IsValid) {
                if (ProductExists(id) == false) {

                    model.CreatedDate = DateTime.Now;
                    _context.Add(model);
                    await _context.SaveChangesAsync();

                } else {
                    var category = await _context.Products.FindAsync(id);

                    category.CategoryProductId = model.CategoryProductId;
                    category.Name = model.Name;
                    category.Price = model.Price;
                    category.Quantity = model.Quantity;
                    category.Display = model.Display;
                    category.Description = model.Description;
                    category.ModifiedDate = DateTime.Now;

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        // POST: Admin/Product/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id) {
            var model = await _context.Products.SingleOrDefaultAsync(m => m.Id == id);

            if (model != null) {
                _context.Products.Remove(model);
                await _context.SaveChangesAsync();

                return Json(new { success = 1 });
            }
            return Json(new { success = 0 });
        }

        private bool ProductExists(int id) {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
