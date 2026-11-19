using CS360DatabaseProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UserEntity = CS360DatabaseProject.Models.Users;

namespace CS360DatabaseProject.Pages.Users;

public class EditModel : PageModel
{
    private readonly CS360DatabaseProjectContext _context;

    public EditModel(CS360DatabaseProjectContext context)
    {
        _context = context;
    }

    [BindProperty] public UserEntity User { get; set; } = new();
    [BindProperty] public string OriginalUserID { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(string? id)
    {
        if (id is null) return NotFound();
        User = await _context.Users.FindAsync(id);
        if (User is null) return NotFound();
        OriginalUserID = User.User_ID;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(OriginalUserID)) return NotFound();
        var existingUser = await _context.Users.FirstOrDefaultAsync(user => user.User_ID == OriginalUserID);
        if (existingUser is null) return NotFound();
        if (!ModelState.IsValid) return Page();

        if (User.User_ID != OriginalUserID &&
            await _context.Users.AnyAsync(user => user.User_ID == User.User_ID))
        {
            ModelState.AddModelError("User.User_ID", "That user ID is already in use.");
            return Page();
        }

        if (User.User_ID == OriginalUserID)
        {
            existingUser.Fname = User.Fname;
            existingUser.Lname = User.Lname;
            existingUser.Email = User.Email;
            existingUser.Password_hash = User.Password_hash;
            existingUser.Active = User.Active;
            existingUser.Created_at = User.Created_at;
            existingUser.Last_Used_at = User.Last_Used_at;
            await _context.SaveChangesAsync();
        }
        else
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            var replacement = new UserEntity
            {
                User_ID = User.User_ID,
                Fname = User.Fname,
                Lname = User.Lname,
                Email = User.Email,
                Password_hash = User.Password_hash,
                Active = User.Active,
                Created_at = User.Created_at,
                Last_Used_at = User.Last_Used_at
            };

            _context.Users.Add(replacement);
            await _context.SaveChangesAsync();
            await _context.PendingOrders
                .Where(order => order.UserID == OriginalUserID)
                .ExecuteUpdateAsync(update => update.SetProperty(order => order.UserID, User.User_ID));
            _context.Users.Remove(existingUser);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        return RedirectToPage("./Index");
    }
}