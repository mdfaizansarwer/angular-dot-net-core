using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        public StoreContext _context;
        public ProductRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ProductBrand>> getProductBrandAsync()
        {
            return await _context.ProductBrands.ToListAsync();
        }

        public async Task<Product> getProductByIdAsync(int id)
        {
            return await _context.Products
            .Include(x => x.ProductBrand)
            .Include(x => x.ProductType)
            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IReadOnlyList<Product>> getProductsAsync()
        {
            return await _context.Products
            .Include(x => x.ProductBrand)
            .Include(x => x.ProductType)
            .ToListAsync();
        }

        public async Task<IReadOnlyList<ProductType>> getProductTypeAsync()
        {
            return await _context.ProductTypes.ToListAsync();
        }
    }
}