using Microsoft.EntityFrameworkCore;
using ProductOrderApi.Data.Entities;

namespace ProductOrderApi.Data.Repositories
{
    public class ProductRepository
    {
        private readonly OrderContext _context;
        public ProductRepository(OrderContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }
        public async Task<Product> GetProduct(int id)
        {
            return await _context.Products.FindAsync(id);
        }
        public async Task<IEnumerable<Product>> GetProductsByIds(IEnumerable<int> ids)
        {
            return await _context.Products.Where(p => ids.Contains(p.Id)).ToListAsync();
        }
        public async Task<Product> AddProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }
        public async Task<Product?> UpdateProduct(Product product)
        {
            if(!await _context.Products.AnyAsync(p => p.Id == product.Id))
            {
                return null;
            }
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }
        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
