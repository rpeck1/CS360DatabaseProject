using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CS360DatabaseProject.Models;
using PendingOrdersModel = CS360DatabaseProject.Models.PendingOrders;

namespace CS360DatabaseProject.Pages.PendingOrders.PendingOrdersPages;

public class DetailsModel : PageModel
{
    private readonly CS360DatabaseProjectContext _context;

    public DetailsModel(CS360DatabaseProjectContext context)
    {
        _context = context;
    }

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
}
