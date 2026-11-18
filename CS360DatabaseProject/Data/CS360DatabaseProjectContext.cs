using Microsoft.EntityFrameworkCore;

public class CS360DatabaseProjectContext(DbContextOptions<CS360DatabaseProjectContext> options) : DbContext(options)
{
    public DbSet<CS360DatabaseProject.Models.Inventory> Inventory { get; set; } = default!;
    public DbSet<CS360DatabaseProject.Models.PendingOrders> PendingOrders { get; set; } = default!;
    public DbSet<CS360DatabaseProject.Models.Users> Users { get; set; } = default!;
    public DbSet<CS360DatabaseProject.Models.Score> Score { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CS360DatabaseProject.Models.Inventory>()
            .HasOne(inventory => inventory.Score)
            .WithMany(score => score.Inventory)
            .HasForeignKey(inventory => inventory.ScoreID)
            .HasPrincipalKey(score => score.ScoreID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CS360DatabaseProject.Models.PendingOrders>()
            .HasOne(order => order.User)
            .WithMany(user => user.PendingOrders)
            .HasForeignKey(order => order.UserID)
            .HasPrincipalKey(user => user.User_ID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CS360DatabaseProject.Models.PendingOrders>()
            .HasOne(order => order.Car)
            .WithMany()
            .HasForeignKey(order => order.CarID)
            .HasPrincipalKey(car => car.CarID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
