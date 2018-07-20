using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Areas.Admin.Controllers {

    [Area("Admin")]
    [Authorize(Roles = ("Mod,Admin"))]
    public class ProductController : Controller {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager) {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        // GET: Admin/Product
        public async Task<IActionResult> Index() {
            return View(await _context.Products.Include(x => x.CategoryProduct).Include(x=>x.User).ToListAsync());
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
        public async Task<IActionResult> Details(int id, [Bind("Id,Name,CategoryProductId,Price,Quantity,Display,Description,ImageUrl")] Product model, IFormFile ImageUrl) {

            if (ModelState.IsValid) {

                string fileName = "";
                // 0. Kiểm tra xem người dùng có chọn file không?
                if (ImageUrl != null && ImageUrl.Length != 0) {
                    // 1. Chọn nơi lưu file
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files");
                    // 2. nếu chưa có, thì tạo mới
                    if (!Directory.Exists(path)) {
                        Directory.CreateDirectory(path);
                    }
                    // 3. Lấy tên file
                    fileName = ImageUrl.FileName;
                    // 4. Lấy đường dẫn và tên file
                    var filePath = Path.Combine(path, fileName);

                    // 5. Tiến hành lưu file
                    using (var stream = new FileStream(filePath, FileMode.Create)) {
                        await ImageUrl.CopyToAsync(stream);
                    }
                }

                if (ProductExists(id) == false) {

                    model.CreatedDate = DateTime.Now;
                    model.UserId = _userManager.GetUserId(HttpContext.User);
                    if (string.IsNullOrEmpty(fileName) == false) {
                        model.ImageUrl = fileName;
                    }

                    _context.Add(model);
                    await _context.SaveChangesAsync();

                } else {
                    var product = await _context.Products.FindAsync(id);

                    product.CategoryProductId = model.CategoryProductId;
                    product.Name = model.Name;
                    product.Price = model.Price;
                    product.Quantity = model.Quantity;
                    product.Display = model.Display;
                    product.Description = model.Description;
                    product.ModifiedDate = DateTime.Now;
                    product.UserId = _userManager.GetUserId(HttpContext.User);
                    if (string.IsNullOrEmpty(fileName) == false) {
                        product.ImageUrl = fileName;
                    }
                    _context.Update(product);
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
