using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAppPOS.Models.Enums;

namespace WebAppPOS.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        public int CustomerId { get; set; } //Foreign key
        public Customer Customer { get; set; } //Navigation Property
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public OrderStatus OrderStatus { get; set; }
        public int TotalProducts { get; set; }
        public int SubTotal { get; set; }
        public int Var { get; set; } // VAT or Variable
        public int Total { get; set; }
        public string InvoiceNo { get; set; }
        public string PaymentType { get; set; }
        public int Pay { get; set; }
        public int Due { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
