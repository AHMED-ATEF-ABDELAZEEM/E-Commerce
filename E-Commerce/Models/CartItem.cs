using E_Commerce.Controllers;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{

    public class CartItem
    {
        
        public string UserId { get; set; }
        [ForeignKey(nameof(Product_ref))]
        public string ProductId { get; set; }
        public int Quantaty { get; set; }
        public virtual Product? Product_ref { get; set; }

    }
}
