using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CS360DatabaseProject.Models;

[Table("Score")]
public class Score
{
    [Key]
    public string ScoreID { get; set; } = string.Empty;

    public int? NHTSA_Score { get; set; }

    public int? GreenCarScore { get; set; }

    public int? Comfort { get; set; }

    [Column("PERFORMANCE")]
    public int? Performance { get; set; }

    public int OverallScore { get; set; }

    public ICollection<Inventory> Inventory { get; set; } = new List<Inventory>();
}
