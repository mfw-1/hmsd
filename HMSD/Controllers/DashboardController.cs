using HMSD.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // BASIC COUNTS
        var students = await _context.Students.CountAsync();
        var rooms = await _context.Rooms.CountAsync();
        var beds = await _context.Beds.CountAsync();
        var allocations = await _context.Allocations.CountAsync();
        var complaints = await _context.Complaints.CountAsync();

        // OCCUPIED BEDS = ACTIVE ALLOCATIONS
        var occupiedBeds = await _context.Allocations
            .Where(a => (int)a.Status == 1) // assuming 1 = active
            .CountAsync();

        // OCCUPANCY %
        var occupancy = beds == 0 ? 0 : (occupiedBeds * 100 / beds);

        // COMPLAINT RESOLUTION %
        var totalComplaints = complaints;

        var resolvedComplaints = await _context.Complaints
            .Where(c => (int)c.Status == 1) // 1 = resolved
            .CountAsync();

        var complaintRate = totalComplaints == 0
            ? 0
            : (resolvedComplaints * 100 / totalComplaints);

        // FEES (temporary logic until payment module is advanced)
        var feeCollection = await _context.StudentFees
            .Where(f => (int)f.Status == 1) // 1 = paid
            .CountAsync();

        var totalFees = await _context.StudentFees.CountAsync();

        var feeRate = totalFees == 0 ? 0 : (feeCollection * 100 / totalFees);

        // PASS DATA TO VIEW
        ViewBag.Students = students;
        ViewBag.Rooms = rooms;
        ViewBag.Beds = beds;
        ViewBag.Allocations = allocations;
        ViewBag.Complaints = complaints;

        ViewBag.Occupancy = occupancy;
        ViewBag.ComplaintRate = complaintRate;
        ViewBag.FeeRate = feeRate;

        return View();
    }
}