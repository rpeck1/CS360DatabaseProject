using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CS360DatabaseProject.Models;
using PendingOrdersModel = CS360DatabaseProject.Models.PendingOrders;

namespace CS360DatabaseProject.Pages.PendingOrders.PendingOrdersPages;

public class DeleteModel : PageModel
{
    private readonly CS360DatabaseProjectContext _context;

    public DeleteModel(CS360DatabaseProjectContext context)
    {
        _context = context;
    }

    [BindProperty]
    public PendingOrdersModel PendingOrders { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(string? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        PendingOrders = await _context.PendingOrders.FirstOrDefaultAsync(m => m.OrderID == id);
        return PendingOrders is null ? NotFound() : Page();
    }

    public async Task<IActionResult> OnPostAsync(string? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var pendingOrder = await _context.PendingOrders.FindAsync(id);
        if (pendingOrder is not null)
        {
            _context.PendingOrders.Remove(pendingOrder);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
