namespace WebAppPOS.Models
{
    public class PurchaseDetail
    {
        public int PurchaseDetailId { get; set; }      
        public int PurchaseId { get; set; }         
        public int ProductId { get; set; }             
        public int Quantity { get; set; }              
        public decimal UnitPrice { get; set; }        
        public decimal TotalPrice => Quantity * UnitPrice;
        // Navigation properties
        public Purchase? Purchases { get; set; }
        public Product Products { get; set; }
    }
}
