using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppPOS.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string? Email { get; set; }
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [RegularExpression(@"^(?:\+855|0)\d{8,9}$",
        ErrorMessage = "Invalid Cambodian phone number")]
        [StringLength(15, MinimumLength = 9)]
        [Display(Name = "Phone Number")]
        public string? Phone { get; set; }
        [StringLength(100)]
        public string? Address { get; set; }
        [StringLength(15)]
        public string? Type { get; set; }
        [StringLength(50)]
        public string? BankName { get; set; }
        [StringLength(50)]
        public string? AccountHolder { get; set; }
        [StringLength(50)]
        public string? AccountNumber { get; set; }
        [StringLength(125)]
        public string? PhotoUrl { get; set; }
        [NotMapped]
        public IFormFile? CustomerImage { get; set; }
        //Navigation Properties
        //public ICollection<Order> Orders { get; set; }
    }
}
