
using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        public Task<IReadOnlyList<Product>> GetProductsAsync();
        public Task<Product> GetProductByIdAsync(int Id);
        
        public Task<IReadOnlyList<ProductType>> GetProductTypesAsync();
        public Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync();
    }
}