using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Data;

namespace Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ("Admin"))]
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoleController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var models = _context.Roles.ToList();
            return View(models);
        }
        public IActionResult Details(string id)
        {
            IdentityRole role = _context.Roles.SingleOrDefault(x => x.Id == id);
            if (role == null)
            {
                role = new IdentityRole();
                return View(role);
            }

            return View(role);
        }

        [HttpPost]
        public IActionResult Details(IdentityRole model)
        {
            var role = _context.Roles.SingleOrDefault(x => x.Id == model.Id);
            if (role == null)
            {
                // add new
                model.NormalizedName = model.Name.ToUpper();
                _context.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            }
            else
            {
                // Edit
                role.Name = model.Name;
                role.NormalizedName = role.Name.ToUpper();
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            var model = _context.Roles.SingleOrDefault(m => m.Id == id);

            if (model != null)
            {
                _context.Roles.Remove(model);
                _context.SaveChanges();

                return Json(new { success = 1 });
            }
            return Json(new { success = 0 });
        }
    }
}