namespace E_Commerce.ViewModel
{
    public class UpdateProductVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public IFormFile? Image { get; set; }
    }
}
