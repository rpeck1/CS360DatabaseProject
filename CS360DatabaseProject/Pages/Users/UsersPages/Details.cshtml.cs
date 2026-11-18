using CS360DatabaseProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserEntity = CS360DatabaseProject.Models.Users;

namespace CS360DatabaseProject.Pages.Users;

public class DetailsModel : PageModel
{
    private readonly CS360DatabaseProjectContext _context;

    public DetailsModel(CS360DatabaseProjectContext context)
    {
        _context = context;
    }

    public UserEntity UserRecord { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(string? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var user = await _context.Users.FindAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        UserRecord = user;
        return Page();
    }
}
