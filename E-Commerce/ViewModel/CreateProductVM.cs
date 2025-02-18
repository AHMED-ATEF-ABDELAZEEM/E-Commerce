using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModel
{
    public class CreateProductVM
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [MinLength(10)]
        [MaxLength(200)]
        public string Description { get; set; }
        [Range(5,500)]
        public int Amount { get; set; }
        [Range(1,100000)]
        public int Price { get; set; }

        [Required]
        public IFormFile Image { get; set; }
        public string CategoryId { get; set; }
        public List<CategoryDropdownVM>? CategoryDropdownList { get; set; }
    }
}
