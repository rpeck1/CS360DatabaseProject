using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CS360DatabaseProject.Models;
using InventoryModel = CS360DatabaseProject.Models.Inventory;

namespace CS360DatabaseProject.Pages.InventoryPages;

public class DeleteModel : PageModel
{
    private readonly CS360DatabaseProjectContext _context;

    public DeleteModel(CS360DatabaseProjectContext context)
    {
        _context = context;
    }

    [BindProperty]
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

    public async Task<IActionResult> OnPostAsync(string? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var inventory = await _context.Inventory.FindAsync(id);
        if (inventory != null)
        {
            Inventory = inventory;
            _context.Inventory.Remove(Inventory);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
