using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HMSD.Data;

namespace HMSD.Controllers
{
    public class AllocationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AllocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Allocations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Allocations.Include(a => a.Bed).Include(a => a.Student);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Allocations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allocation = await _context.Allocations
                .Include(a => a.Bed)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (allocation == null)
            {
                return NotFound();
            }

            return View(allocation);
        }

        // GET: Allocations/Create
        public IActionResult Create()
        {
            ViewData["BedId"] = new SelectList(_context.Beds, "Id", "BedNumber");
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Address");
            return View();
        }

        // POST: Allocations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,BedId,AllocatedDate,ExpectedCheckoutDate,ActualCheckoutDate,Status,Remarks,Id,CreatedAt,UpdatedAt")] Allocation allocation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(allocation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BedId"] = new SelectList(_context.Beds, "Id", "BedNumber", allocation.BedId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Address", allocation.StudentId);
            return View(allocation);
        }

        // GET: Allocations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allocation = await _context.Allocations.FindAsync(id);
            if (allocation == null)
            {
                return NotFound();
            }
            ViewData["BedId"] = new SelectList(_context.Beds, "Id", "BedNumber", allocation.BedId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Address", allocation.StudentId);
            return View(allocation);
        }

        // POST: Allocations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,BedId,AllocatedDate,ExpectedCheckoutDate,ActualCheckoutDate,Status,Remarks,Id,CreatedAt,UpdatedAt")] Allocation allocation)
        {
            if (id != allocation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(allocation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AllocationExists(allocation.Id))
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
            ViewData["BedId"] = new SelectList(_context.Beds, "Id", "BedNumber", allocation.BedId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Address", allocation.StudentId);
            return View(allocation);
        }

        // GET: Allocations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allocation = await _context.Allocations
                .Include(a => a.Bed)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (allocation == null)
            {
                return NotFound();
            }

            return View(allocation);
        }

        // POST: Allocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var allocation = await _context.Allocations.FindAsync(id);
            if (allocation != null)
            {
                _context.Allocations.Remove(allocation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AllocationExists(int id)
        {
            return _context.Allocations.Any(e => e.Id == id);
        }
    }
}
