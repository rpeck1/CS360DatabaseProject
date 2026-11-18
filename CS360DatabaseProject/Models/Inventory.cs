using System.ComponentModel.DataAnnotations;

namespace CS360DatabaseProject.Models
{
    public class Inventory
    {
        [Key]
        public string CarID { get; set; } = string.Empty;

        public string Make { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public short Year { get; set; }

        public string Color { get; set; } = string.Empty;

        public string? ScoreID { get; set; }

        public Score? Score { get; set; }

        public double BasePrice { get; set; }
    }
}
