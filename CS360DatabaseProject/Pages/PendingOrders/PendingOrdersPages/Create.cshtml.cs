using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using CS360DatabaseProject.Models;
using PendingOrdersModel = CS360DatabaseProject.Models.PendingOrders;

namespace CS360DatabaseProject.Pages.PendingOrders.PendingOrdersPages;

public class CreateModel : PageModel
{
    private readonly CS360DatabaseProjectContext _context;

    public CreateModel(CS360DatabaseProjectContext context)
    {
        _context = context;
    }

    [BindProperty]
    public PendingOrdersModel PendingOrders { get; set; } = default!;

    public IEnumerable<SelectListItem> UserOptions { get; set; } = [];

    public IEnumerable<SelectListItem> CarOptions { get; set; } = [];

    public async Task OnGetAsync()
    {
        await LoadOptionsAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadOptionsAsync();
            return Page();
        }

        _context.PendingOrders.Add(PendingOrders);
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