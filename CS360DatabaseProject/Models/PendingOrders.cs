using System.ComponentModel.DataAnnotations;

namespace CS360DatabaseProject.Models
{
    public class PendingOrders
    {
        [Key]
        public string OrderID { get; set; } = string.Empty;

        public string UserID { get; set; } = string.Empty;

        public Users? User { get; set; }

        public string CarID { get; set; } = string.Empty;

        public Inventory? Car { get; set; }

        public string? AddOns { get; set; }

        public DateTime? ETA { get; set; }

        public bool? InStock { get; set; }

        public string? Address { get; set; }
    }
}
