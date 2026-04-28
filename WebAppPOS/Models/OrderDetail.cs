using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppPOS.Models
{
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string OrderId { get; set; }
        public virtual Order Order { get; set; }

        public string ProductId { get; set; } //Foreign key to Product
        public virtual Product Product { get; set; } //Navigation property to Product

        public int Quantity { get; set; }
        public int Unitcost { get; set; }
        public int Total { get; set; }
    }
}
