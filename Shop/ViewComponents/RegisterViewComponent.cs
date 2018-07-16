using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.ViewComponents {
    public class RegisterViewComponent : ViewComponent {

        private readonly ApplicationDbContext _context;

        public RegisterViewComponent(ApplicationDbContext context) {
            _context = context;
        }

        public IViewComponentResult Invoke() {
            
            return View();
        }
    }
}
