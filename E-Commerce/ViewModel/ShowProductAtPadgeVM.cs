namespace E_Commerce.ViewModel
{
    public class ShowProductAtPadgeVM
    {
        public int CurrentPadge { get; set; }
        public int PadgeSize { get; set; }
        public int CountOfPadge { get; set; }
        public string Category { get; set; }
        public List<ShowProductVM> Products { get; set; }
        public List<CategoryDropdownVM>? CategoryDropdownList { get; set; }
    }
}
