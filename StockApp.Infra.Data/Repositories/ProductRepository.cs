using StockApp.Domain.Entities;
using StockApp.Domain.Interfaces;
using StockApp.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace StockApp.Infra.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        ApplicationDbContext _productContext;
        public ProductRepository(ApplicationDbContext context)
        {
            _productContext = context;
        }

        public async Task<Product> Create(Product product)
        {
            _productContext.Add(product);
            await _productContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product> GetById(int? id)
        {
            return await _productContext.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _productContext.Products.ToListAsync();
        }

        public async Task<Product> Remove(Product product)
        {
            _productContext.Remove(product);
            await _productContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product> Update(Product product)
        {
            _productContext.Update(product);
            await _productContext.SaveChangesAsync();
            return product;
        }
        public async Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await _productContext.Products.Where(p => ids.Contains(p.Id)).ToListAsync();
        }

        public async Task<int> GetStockAsync(int productId)
        {
            var product = await _productContext.Products.FindAsync(productId);
            return product != null ? product.Stock : 0;
        }

        public async Task UpdateStockAsync(int productId, int newStock)
        {
            var product = await _productContext.Products.FindAsync(productId);
            if (product != null)
            {
                product.Stock = newStock;
                await _productContext.SaveChangesAsync();
            }
        }
    }
}
