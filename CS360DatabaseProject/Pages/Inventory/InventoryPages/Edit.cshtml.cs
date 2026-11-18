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
        await LoadScoreOptionsAsync();
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadScoreOptionsAsync();
            return Page();
        }

        _context.Attach(Inventory).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!InventoryExists(Inventory.CarID))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
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

    private bool InventoryExists(string id)
    {
        return _context.Inventory.Any(e => e.CarID == id);
    }
}
