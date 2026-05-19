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
    public class StudentFeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentFeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StudentFees
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.StudentFees.Include(s => s.FeeType).Include(s => s.Student);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: StudentFees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentFee = await _context.StudentFees
                .Include(s => s.FeeType)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentFee == null)
            {
                return NotFound();
            }

            return View(studentFee);
        }

        // GET: StudentFees/Create
        public IActionResult Create()
        {
            ViewData["FeeTypeId"] = new SelectList(_context.FeeTypes, "Id", "Name");
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Address");
            return View();
        }

        // POST: StudentFees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,FeeTypeId,Amount,DueDate,Status,Month,Year,Id,CreatedAt,UpdatedAt")] StudentFee studentFee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studentFee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FeeTypeId"] = new SelectList(_context.FeeTypes, "Id", "Name", studentFee.FeeTypeId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Address", studentFee.StudentId);
            return View(studentFee);
        }

        // GET: StudentFees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentFee = await _context.StudentFees.FindAsync(id);
            if (studentFee == null)
            {
                return NotFound();
            }
            ViewData["FeeTypeId"] = new SelectList(_context.FeeTypes, "Id", "Name", studentFee.FeeTypeId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Address", studentFee.StudentId);
            return View(studentFee);
        }

        // POST: StudentFees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,FeeTypeId,Amount,DueDate,Status,Month,Year,Id,CreatedAt,UpdatedAt")] StudentFee studentFee)
        {
            if (id != studentFee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentFee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentFeeExists(studentFee.Id))
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
            ViewData["FeeTypeId"] = new SelectList(_context.FeeTypes, "Id", "Name", studentFee.FeeTypeId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Address", studentFee.StudentId);
            return View(studentFee);
        }

        // GET: StudentFees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentFee = await _context.StudentFees
                .Include(s => s.FeeType)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentFee == null)
            {
                return NotFound();
            }

            return View(studentFee);
        }

        // POST: StudentFees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentFee = await _context.StudentFees.FindAsync(id);
            if (studentFee != null)
            {
                _context.StudentFees.Remove(studentFee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentFeeExists(int id)
        {
            return _context.StudentFees.Any(e => e.Id == id);
        }
    }
}
