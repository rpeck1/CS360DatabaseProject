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

    [BindProperty]
    public string OriginalOrderID { get; set; } = string.Empty;

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

        OriginalOrderID = PendingOrders.OrderID;
        await LoadOptionsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(OriginalOrderID))
        {
            return NotFound();
        }

        var existingOrder = await _context.PendingOrders
            .FirstOrDefaultAsync(order => order.OrderID == OriginalOrderID);
        if (existingOrder is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            await LoadOptionsAsync();
            return Page();
        }

        if (PendingOrders.OrderID != OriginalOrderID &&
            await _context.PendingOrders.AnyAsync(order => order.OrderID == PendingOrders.OrderID))
        {
            ModelState.AddModelError("PendingOrders.OrderID", "That order ID is already in use.");
            await LoadOptionsAsync();
            return Page();
        }

        if (PendingOrders.OrderID == OriginalOrderID)
        {
            existingOrder.UserID = PendingOrders.UserID;
            existingOrder.CarID = PendingOrders.CarID;
            existingOrder.AddOns = PendingOrders.AddOns;
            existingOrder.ETA = PendingOrders.ETA;
            existingOrder.InStock = PendingOrders.InStock;
            existingOrder.Address = PendingOrders.Address;
            await _context.SaveChangesAsync();
        }
        else
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            var replacement = new PendingOrdersModel
            {
                OrderID = PendingOrders.OrderID,
                UserID = PendingOrders.UserID,
                CarID = PendingOrders.CarID,
                AddOns = PendingOrders.AddOns,
                ETA = PendingOrders.ETA,
                InStock = PendingOrders.InStock,
                Address = PendingOrders.Address
            };

            _context.PendingOrders.Add(replacement);
            await _context.SaveChangesAsync();
            _context.PendingOrders.Remove(existingOrder);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }

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