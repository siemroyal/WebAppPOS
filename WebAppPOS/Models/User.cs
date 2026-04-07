using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppPOS.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        public string Name { get; set; }
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string? PhotoUrl { get; set; }
        [NotMapped]
        public IFormFile? UserImage { get; set; }
        // Navigation properties
        public ICollection<Purchase>? CreatedPurchases { get; set; }
        public ICollection<Purchase>? UpdatedPurchases { get; set; }

    }
}
