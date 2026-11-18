using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CS360DatabaseProject.Models;
using InventoryModel = CS360DatabaseProject.Models.Inventory;

namespace CS360DatabaseProject.Pages.InventoryPages;

public class IndexModel : PageModel
{
    private readonly CS360DatabaseProjectContext _context;

    public IndexModel(CS360DatabaseProjectContext context)
    {
        _context = context;
    }

    public IList<InventoryModel> Inventory { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Inventory = await _context.Inventory.ToListAsync();
    }
}
