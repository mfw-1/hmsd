using HMSD.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    // PROFILE PAGE
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);

        return View(user);
    }

    // UPDATE PROFILE
    [HttpPost]
    public async Task<IActionResult> Profile(ApplicationUser model)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return NotFound();

        user.FullName = model.FullName;
        user.Email = model.Email;
        user.UserName = model.Email;
        user.PhoneNumber = model.PhoneNumber;

        await _userManager.UpdateAsync(user);

        ViewBag.Success = "Profile updated successfully";

        return View(user);
    }
}