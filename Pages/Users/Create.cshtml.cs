using CS360DatabaseProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserEntity = CS360DatabaseProject.Models.Users;

namespace CS360DatabaseProject.Pages.Users;

public class CreateModel : PageModel
{
    private readonly CS360DatabaseProjectContext _context;

    public CreateModel(CS360DatabaseProjectContext context)
    {
        _context = context;
    }

    [BindProperty] public UserEntity User { get; set; } = new();

    public void OnGet() { User.Created_at = DateTime.UtcNow; User.Last_Used_at = DateTime.UtcNow; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        _context.Users.Add(User);
        await _context.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
}