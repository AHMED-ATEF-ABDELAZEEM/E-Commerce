using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModel.AcountVM
{
    public class CustomerProfileVM
    {
        [MinLength(3, ErrorMessage = "Region must be at least 3 characters.")]
        [MaxLength(20, ErrorMessage = "Region must not exceed 20 characters.")]
        public string? Region { get; set; }

        [MinLength(3, ErrorMessage = "City must be at least 3 characters.")]
        [MaxLength(20, ErrorMessage = "City must not exceed 20 characters.")]
        public string? City { get; set; }

        [MinLength(3, ErrorMessage = "Area must be at least 3 characters.")]
        [MaxLength(50, ErrorMessage = "Area must not exceed 50 characters.")]
        public string? Area { get; set; }

        [MinLength(3, ErrorMessage = "Street must be at least 3 characters.")]
        [MaxLength(50, ErrorMessage = "Street must not exceed 50 characters.")]
        public string? Street { get; set; }
    }

}
