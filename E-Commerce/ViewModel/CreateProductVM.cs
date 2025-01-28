﻿namespace E_Commerce.ViewModel
{
    public class CreateProductVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public int Price { get; set; }
        public int CategoryId { get; set; }
        public List<CategoryDropdownVM>? CategoryDropdownList { get; set; }
    }
}
