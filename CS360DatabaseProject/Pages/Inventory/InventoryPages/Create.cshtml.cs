using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using CS360DatabaseProject.Models;
using InventoryModel = CS360DatabaseProject.Models.Inventory;

namespace CS360DatabaseProject.Pages.InventoryPages;

public class CreateModel : PageModel
{
    private readonly CS360DatabaseProjectContext _context;

    public CreateModel(CS360DatabaseProjectContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public InventoryModel Inventory { get; set; } = default!;

    public IEnumerable<SelectListItem> ScoreOptions { get; set; } = [];

    public async Task OnGetAsync()
    {
        await LoadScoreOptionsAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadScoreOptionsAsync();
            return Page();
        }

        _context.Inventory.Add(Inventory);
        await _context.SaveChangesAsync();

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
