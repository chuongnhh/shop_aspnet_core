using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.ViewComponents {
    public class LoginViewComponent : ViewComponent {

        private readonly ApplicationDbContext _context;

        public LoginViewComponent(ApplicationDbContext context) {
            _context = context;
        }

        public IViewComponentResult Invoke() {
            
            return View();
        }
    }
}
