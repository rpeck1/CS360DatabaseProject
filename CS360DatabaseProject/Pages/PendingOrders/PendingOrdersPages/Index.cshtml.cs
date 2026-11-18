using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CS360DatabaseProject.Models;
using PendingOrdersModel = CS360DatabaseProject.Models.PendingOrders;

namespace CS360DatabaseProject.Pages.PendingOrders.PendingOrdersPages;

public class IndexModel : PageModel
{
    private readonly CS360DatabaseProjectContext _context;

    public IndexModel(CS360DatabaseProjectContext context)
    {
        _context = context;
    }

    public IList<PendingOrdersModel> PendingOrders { get; set; } = default!;

    public async Task OnGetAsync()
    {
        PendingOrders = await _context.PendingOrders.ToListAsync();
    }
}