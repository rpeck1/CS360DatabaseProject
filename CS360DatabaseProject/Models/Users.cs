using System.ComponentModel.DataAnnotations;

namespace CS360DatabaseProject.Models
{
    public class Users
    {
        [Key]
        public string User_ID { get; set; } = string.Empty;

        public string Fname { get; set; } = string.Empty;

        public string Lname { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password_hash { get; set; } = string.Empty;

        public bool Active { get; set; }

        public DateTime Created_at { get; set; }

        public DateTime Last_Used_at { get; set; }

        public ICollection<PendingOrders> PendingOrders { get; set; } = new List<PendingOrders>();
    }
}
