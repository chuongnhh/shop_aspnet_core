using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Areas.Admin.Controllers {
    [Area("Admin")]

    [Authorize(Roles = ("Mod,Admin"))]
    public class CategoryProductController : Controller {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CategoryProductController(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager) {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin/CategoryProduct
        public async Task<IActionResult> Index() {
            return View(await _context.CategoryProducts.Include(x=>x.User).Include(x => x.GroupProduct).ToListAsync());
        }

        // GET: Admin/CategoryProduct/Edit/5
        public async Task<IActionResult> Details(int? id) {

            var model = await _context.CategoryProducts.SingleOrDefaultAsync(m => m.Id == id);

            if (model == null) {
                model = new CategoryProduct();
                ViewBag.Groups = new SelectList(_context.GroupProducts.ToList(), "Id", "Name");
                return View(model);
            }
            ViewBag.Groups = new SelectList(_context.GroupProducts.ToList(), "Id", "Name", model.GroupProductId);
            return View(model);
        }

        // POST: Admin/CategoryProduct/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int id, [Bind("Id,Name,GroupProductId")] CategoryProduct model) {

            if (ModelState.IsValid) {
                if (CategoryProductExists(id) == false) {

                    model.CreatedDate = DateTime.Now;
                    model.UserId = _userManager.GetUserId(HttpContext.User);
                    _context.Add(model);
                    await _context.SaveChangesAsync();

                } else {
                    var category = await _context.CategoryProducts.FindAsync(id);

                    category.GroupProductId = model.GroupProductId;
                    category.Name = model.Name;
                    category.ModifiedDate = DateTime.Now;
                    category.UserId = _userManager.GetUserId(HttpContext.User);

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        // POST: Admin/CategoryProduct/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id) {
            var model = await _context.CategoryProducts.SingleOrDefaultAsync(m => m.Id == id);

            if (model != null) {
                _context.CategoryProducts.Remove(model);
                await _context.SaveChangesAsync();

                return Json(new { success = 1 });
            }
            return Json(new { success = 0 });
        }

        private bool CategoryProductExists(int id) {
            return _context.CategoryProducts.Any(e => e.Id == id);
        }
    }
}
