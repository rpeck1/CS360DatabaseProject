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

    [BindProperty] public UserEntity UserRecord { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(string? id)
    {
        if (id is null) return NotFound();
        var user = await _context.Users.FindAsync(id);
        if (user is null) return NotFound();
        UserRecord = user;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string? id)
    {
        if (id is null || id != UserRecord.User_ID) return NotFound();
        if (!ModelState.IsValid) return Page();
        _context.Attach(UserRecord).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
}