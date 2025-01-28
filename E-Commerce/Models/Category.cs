namespace E_Commerce.Models
{
    public class Category
    {
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CountType { get; set; }
        public int Amount { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

        public virtual ICollection<Product> Product_ref { get; set; }

    }
}
