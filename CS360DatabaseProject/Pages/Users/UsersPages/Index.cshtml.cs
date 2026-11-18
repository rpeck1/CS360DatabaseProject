using CS360DatabaseProject.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UserEntity = CS360DatabaseProject.Models.Users;

namespace CS360DatabaseProject.Pages.Users;

public class IndexModel : PageModel
{
    private readonly CS360DatabaseProjectContext _context;

    public IndexModel(CS360DatabaseProjectContext context)
    {
        _context = context;
    }

    public IList<UserEntity> Users { get; set; } = default!;

    public async Task OnGetAsync() => Users = await _context.Users.AsNoTracking().ToListAsync();
}