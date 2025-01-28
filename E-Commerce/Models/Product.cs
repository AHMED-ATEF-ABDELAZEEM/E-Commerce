using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public int Price { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

        [ForeignKey("Product_ref")]
        public int CategoryId { get; set; }
        public virtual Category Category_ref { get; set; }
    }
}
