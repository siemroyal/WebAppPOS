using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppPOS.Models
{
    public class Unit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UnitId { get; set; }
        [Required]
        [StringLength(50)]
        public string UnitName { get; set; }
        [StringLength(200)]
        public string? Description { get; set; }
        public ICollection<Product> Products { get; set; } //Navigation Property
    }
}
