namespace E_Commerce.ViewModel
{
    public class getProductAtHomePadgeVM
    {
        public PadgeInformationVM PadgeInformation { get; set; }
        public List<ProductAtHomeVM> Products { get; set; }
        public List<CategoryDropdownVM>? CategoryDropdownList { get; set; }
    }
}
