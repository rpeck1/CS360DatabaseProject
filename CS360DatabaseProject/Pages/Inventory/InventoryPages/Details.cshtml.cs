using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CS360DatabaseProject.Models;
using InventoryModel = CS360DatabaseProject.Models.Inventory;

namespace CS360DatabaseProject.Pages.InventoryPages;

public class DetailsModel : PageModel
{
    private readonly CS360DatabaseProjectContext _context;
    public DetailsModel(CS360DatabaseProjectContext context)
    {
        _context = context;
    }

    public InventoryModel Inventory { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(string? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var inventory = await _context.Inventory.FirstOrDefaultAsync(m => m.CarID == id);
        if (inventory is null)
        {
            return NotFound();
        }
        else
        {
            Inventory = inventory;
        }

        return Page();
    }
}
