using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppPOS.Models
{
public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Product Name is required")]
        [StringLength(125)]
        public string ProductName { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "Product Code is required")]
        public string ProductCode { get; set; }
        [Required(ErrorMessage = "Cost is required")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode =true)]
        public decimal Cost { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Category is required")]
        [DefaultValue(0)]
        public int StockQty { get; set; }
        public string? ProductUrl { get; set; }
        [NotMapped]
        public IFormFile? ProudctImage { get; set; }
        public int CategoryId { get; set; } //Foriegn Key
        public Category? Category { get; set; } //Navigation Property
        public int UnitId { get; set; } //Foriegn Key
        //Navigation Property
        public Unit? Unit { get; set; }
        public ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();
    }
}
