using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models
{
    public class WishList
    {
        [ForeignKey(nameof(Customer_ref))]
        public string CustomerId { get; set; }
        [ForeignKey(nameof(Product_ref))]
        public string ProductId { get; set; }
        public virtual Product Product_ref {  get; set; }
        public virtual CustomerProfile Customer_ref { get; set; }
    }
}
