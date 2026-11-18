using CS360DatabaseProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UserEntity = CS360DatabaseProject.Models.Users;

namespace CS360DatabaseProject.Pages.Users;

public class EditModel : PageModel
{
    private readonly CS360DatabaseProjectContext _context;

    public EditModel(CS360DatabaseProjectContext context)
    {
        _context = context;
    }

    [BindProperty] public UserEntity User { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(string? id)
    {
        if (id is null) return NotFound();
        User = await _context.Users.FindAsync(id);
        return User is null ? NotFound() : Page();
    }

    public async Task<IActionResult> OnPostAsync(string? id)
    {
        if (id is null || id != User.User_ID) return NotFound();
        if (!ModelState.IsValid) return Page();
        _context.Attach(User).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
}