using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using CS360DatabaseProject.Models;
using InventoryModel = CS360DatabaseProject.Models.Inventory;

namespace CS360DatabaseProject.Pages.InventoryPages;

public class EditModel : PageModel
{
    private readonly CS360DatabaseProjectContext _context;

    public EditModel(CS360DatabaseProjectContext context)
    {
        _context = context;
    }

    [BindProperty]
    public InventoryModel Inventory { get; set; } = default!;

    [BindProperty]
    public string OriginalCarID { get; set; } = string.Empty;

    public IEnumerable<SelectListItem> ScoreOptions { get; set; } = [];

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
        Inventory = inventory;
        OriginalCarID = inventory.CarID;
        await LoadScoreOptionsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(OriginalCarID))
        {
            return NotFound();
        }

        var existingInventory = await _context.Inventory
            .FirstOrDefaultAsync(car => car.CarID == OriginalCarID);
        if (existingInventory is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            await LoadScoreOptionsAsync();
            return Page();
        }

        if (Inventory.CarID != OriginalCarID &&
            await _context.Inventory.AnyAsync(car => car.CarID == Inventory.CarID))
        {
            ModelState.AddModelError("Inventory.CarID", "That vehicle ID is already in use.");
            await LoadScoreOptionsAsync();
            return Page();
        }

        if (Inventory.CarID == OriginalCarID)
        {
            existingInventory.Make = Inventory.Make;
            existingInventory.Model = Inventory.Model;
            existingInventory.Year = Inventory.Year;
            existingInventory.Color = Inventory.Color;
            existingInventory.ScoreID = Inventory.ScoreID;
            existingInventory.BasePrice = Inventory.BasePrice;
            await _context.SaveChangesAsync();
        }
        else
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            var replacement = new InventoryModel
            {
                CarID = Inventory.CarID,
                Make = Inventory.Make,
                Model = Inventory.Model,
                Year = Inventory.Year,
                Color = Inventory.Color,
                ScoreID = Inventory.ScoreID,
                BasePrice = Inventory.BasePrice
            };

            _context.Inventory.Add(replacement);
            await _context.SaveChangesAsync();
            await _context.PendingOrders
                .Where(order => order.CarID == OriginalCarID)
                .ExecuteUpdateAsync(update => update.SetProperty(order => order.CarID, Inventory.CarID));
            _context.Inventory.Remove(existingInventory);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        return RedirectToPage("./Index");
    }

    private async Task LoadScoreOptionsAsync()
    {
        ScoreOptions = await _context.Score
            .AsNoTracking()
            .OrderBy(score => score.ScoreID)
            .Select(score => new SelectListItem
            {
                Value = score.ScoreID,
                Text = $"{score.ScoreID} (Overall: {score.OverallScore})"
            })
            .ToListAsync();
    }

}
