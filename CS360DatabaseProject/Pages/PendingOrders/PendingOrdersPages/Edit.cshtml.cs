using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using CS360DatabaseProject.Models;
using PendingOrdersModel = CS360DatabaseProject.Models.PendingOrders;

namespace CS360DatabaseProject.Pages.PendingOrders.PendingOrdersPages;

public class EditModel : PageModel
{
    private readonly CS360DatabaseProjectContext _context;

    public EditModel(CS360DatabaseProjectContext context)
    {
        _context = context;
    }

    [BindProperty]
    public PendingOrdersModel PendingOrders { get; set; } = default!;

    public IEnumerable<SelectListItem> UserOptions { get; set; } = [];

    public IEnumerable<SelectListItem> CarOptions { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(string? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        PendingOrders = await _context.PendingOrders.FindAsync(id);
        if (PendingOrders is null)
        {
            return NotFound();
        }

        await LoadOptionsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string? id)
    {
        if (id is null || id != PendingOrders.OrderID)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            await LoadOptionsAsync();
            return Page();
        }

        _context.Attach(PendingOrders).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return RedirectToPage("./Index");
    }

    private async Task LoadOptionsAsync()
    {
        UserOptions = await _context.Users
            .AsNoTracking()
            .OrderBy(user => user.User_ID)
            .Select(user => new SelectListItem
            {
                Value = user.User_ID,
                Text = $"{user.User_ID} - {user.Fname} {user.Lname}"
            })
            .ToListAsync();

        CarOptions = await _context.Inventory
            .AsNoTracking()
            .OrderBy(car => car.CarID)
            .Select(car => new SelectListItem
            {
                Value = car.CarID,
                Text = $"{car.CarID} - {car.Make} {car.Model}"
            })
            .ToListAsync();
    }
}