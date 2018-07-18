using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;
using Shop.Models.AccountViewModels;

namespace Shop.Areas.Admin.Controllers
{
    [Area("Admin")]

    [Authorize(Roles = ("Admin"))]

    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var models = _context.Users.ToList();
            ViewBag.Roles = _context.Roles.ToList();
            ViewBag.UserRoles = _context.UserRoles.ToList();
            return View(models);
        }
        public IActionResult Details(string id)
        {
            IdentityUser user = _context.Users.SingleOrDefault(x => x.Id == id);
            var model = new RegisterViewModel();

            ViewBag.Roles = _context.Roles.ToList();
            ViewBag.UserRoles = new List<IdentityUserRole<string>>();
            if (user == null)
            {

                return View(model);
            }
            ViewBag.UserRoles = _context.UserRoles.Where(x => x.UserId == user.Id).ToList();
            model = new RegisterViewModel { Email = user.Email, Id = user.Id };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Details(RegisterViewModel model, string[] SelectRoles)
        {
            var user = _context.Users.SingleOrDefault(x => x.Id == model.Id);
            if (user == null)
            {
                user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, "User@123");

                foreach (var item in SelectRoles)
                {
                    var role = _context.Roles.SingleOrDefault(x => x.Id == item);
                    _userManager.AddToRoleAsync(user, role.NormalizedName).Wait();
                }
            }
            else
            {
                // Edit
                if (string.IsNullOrEmpty(model.Email) == false)
                {
                    await _userManager.SetEmailAsync(user, model.Email);
                    await _userManager.SetUserNameAsync(user, model.Email);
                }

                // delete before add
                _context.UserRoles.RemoveRange(_context.UserRoles.Where(x => x.UserId == user.Id));

                foreach (var item in SelectRoles)
                {
                    // check user have this role
                    var userRole = _context.UserRoles.Any(x => x.UserId == user.Id && x.RoleId == item);
                    if (userRole == false)
                    {
                        //
                        var role = _context.Roles.SingleOrDefault(x => x.Id == item);
                        _userManager.AddToRoleAsync(user, role.NormalizedName).Wait();
                    }
                }
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            var model = _context.Users.SingleOrDefault(m => m.Id == id);

            if (model != null)
            {
                _context.Users.Remove(model);
                _context.SaveChanges();

                return Json(new { success = 1 });
            }
            return Json(new { success = 0 });
        }
    }
}