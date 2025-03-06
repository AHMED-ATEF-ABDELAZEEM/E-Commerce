using E_Commerce.Controllers;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{

    public class CartItem
    {

        [ForeignKey(nameof(CustomerProfile_ref))]
        public string UserId { get; set; }
        [ForeignKey(nameof(Product_ref))]
        public string ProductId { get; set; }
        public int Quantaty { get; set; }
        public virtual Product? Product_ref { get; set; }
        public virtual CustomerProfile? CustomerProfile_ref { get; set; }

    }
}
