using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAppPOS.Models.Enums;

namespace WebAppPOS.Models
{
    public class Purchase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseId { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
        [Required]
        public string PurchaseNo { get; set; }
        [Required]
        public PurchaseStatus PurchaseStatus { get; set; }
        public int? SupplierId { get; set; } //Foreign key to Supplier
        public int? CreatedBy { get; set; } //Foreign key to User
        public int? UpdatedBy { get; set; } //Foreign key to User
        // Navigation properties
        [ForeignKey("SupplierId")]
        public Supplier? Suppliers { get; set; }

        [ForeignKey("CreatedBy")]
        public User? CreatedUser { get; set; }

        [ForeignKey("UpdatedBy")]
        public User? UpdatedUser { get; set; }

        public ICollection<PurchaseDetail> PurchaseDetails { get; set; }
    }
}
