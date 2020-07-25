using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> getProductByIdAsync(int id);
        Task<IReadOnlyList<Product>> getProductsAsync();
        Task<IReadOnlyList<ProductBrand>> getProductBrandAsync();
        Task<IReadOnlyList<ProductType>> getProductTypeAsync();
    }
}