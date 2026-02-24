using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppPOS.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; } //ModelId ex. CategoryId,Id, CategoryID, Categoryid
        [Required]
        [StringLength(75)]
        [Column("category_name")]
        public string CategoryName { get; set; }
        public string? ImageUrl { get; set; } = string.Empty;
        [NotMapped]
        public IFormFile? FileImage { get; set; }
        public string Description { get; set; } = string.Empty;
        public ICollection<Product> Products { get; set; } //Navigation Property
    }
}
