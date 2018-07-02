using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop.Data;

namespace Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GroupProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GroupProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/GroupProduct
        public async Task<IActionResult> Index()
        {
            return View(await _context.GroupProducts.ToListAsync());
        }

        // GET: Admin/GroupProduct/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupProduct = await _context.GroupProducts
                .SingleOrDefaultAsync(m => m.Id == id);
            if (groupProduct == null)
            {
                return NotFound();
            }

            return View(groupProduct);
        }

        // GET: Admin/GroupProduct/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/GroupProduct/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] GroupProduct groupProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(groupProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(groupProduct);
        }

        // GET: Admin/GroupProduct/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupProduct = await _context.GroupProducts.SingleOrDefaultAsync(m => m.Id == id);
            if (groupProduct == null)
            {
                return NotFound();
            }
            return View(groupProduct);
        }

        // POST: Admin/GroupProduct/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] GroupProduct groupProduct)
        {
            if (id != groupProduct.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groupProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupProductExists(groupProduct.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(groupProduct);
        }

        // GET: Admin/GroupProduct/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupProduct = await _context.GroupProducts
                .SingleOrDefaultAsync(m => m.Id == id);
            if (groupProduct == null)
            {
                return NotFound();
            }

            return View(groupProduct);
        }

        // POST: Admin/GroupProduct/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groupProduct = await _context.GroupProducts.SingleOrDefaultAsync(m => m.Id == id);
            _context.GroupProducts.Remove(groupProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupProductExists(int id)
        {
            return _context.GroupProducts.Any(e => e.Id == id);
        }
    }
}
