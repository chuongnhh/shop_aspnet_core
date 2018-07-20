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
    public class GroupProductController : Controller {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GroupProductController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager) {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin/GroupProduct
        public async Task<IActionResult> Index() {
            return View(await _context.GroupProducts.Include(x => x.User).ToListAsync());
        }

        // GET: Admin/GroupProduct/Edit/5
        public async Task<IActionResult> Details(int? id) {

            var model = await _context.GroupProducts.SingleOrDefaultAsync(m => m.Id == id);

            if (model == null) {
                model = new GroupProduct();
                return View(model);
            }

            return View(model);
        }

        // POST: Admin/GroupProduct/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int id, [Bind("Id,Name")] GroupProduct model) {

            if (ModelState.IsValid) {
                if (GroupProductExists(id) == false) {

                    model.CreatedDate = DateTime.Now;
                    model.UserId = _userManager.GetUserId(HttpContext.User);
                    _context.Add(model);
                    await _context.SaveChangesAsync();

                } else {
                    var group = await _context.GroupProducts.FindAsync(id);

                    group.Name = model.Name;
                    group.ModifiedDate = DateTime.Now;
                    group.UserId = _userManager.GetUserId(HttpContext.User);
                    _context.Update(group);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        // POST: Admin/GroupProduct/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id) {
            var model = await _context.GroupProducts.SingleOrDefaultAsync(m => m.Id == id);

            if (model != null) {
                _context.GroupProducts.Remove(model);
                await _context.SaveChangesAsync();

                return Json(new { success = 1 });
            }
            return Json(new { success = 0 });
        }

        private bool GroupProductExists(int id) {
            return _context.GroupProducts.Any(e => e.Id == id);
        }
    }
}
