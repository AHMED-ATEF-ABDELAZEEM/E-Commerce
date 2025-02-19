using E_Commerce.ViewModel;

namespace E_Commerce.Service
{
    public interface IHomeService
    {
        Task<List<ProductAtHomeVM>> getAllProducts();

        Task <ProductInformationAtHomeVM> getProductInformation (string productId);
    }
}
