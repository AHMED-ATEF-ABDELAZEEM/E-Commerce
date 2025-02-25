using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models
{
    public class CustomerProfile
    {
        [Key,ForeignKey("User")]
        public string CustomerId { get; set; }
        public string? Region { get; set; }   // المحافظة 
        public string? City { get; set; }     // المركز 
        public string? Area { get; set; }     // القرية 
        public string? Street { get; set; }   // الشارع
        public int WishlistCount { get; set; }
        public int CartCount { get; set; }
        public bool IsActive { get; set; } // At least Make one order

        public virtual ApplicationUser? User { get; set; }
    }

}
