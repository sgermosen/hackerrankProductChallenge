using ProductOrderApi.Data.Entities;
using ProductOrderApi.Data.Repositories;

namespace ProductOrderApi.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;
        public ProductService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _productRepository.GetProducts();
        }
        public async Task<Product> GetProduct(int id)
        {
            return await _productRepository.GetProduct(id);
        }
        public async Task<IEnumerable<Product>> GetProductsByIds(IEnumerable<int> ids)
        {
            return await _productRepository.GetProductsByIds(ids);
        }
        public async Task<Product> AddProduct(Product product)
        {
            return await _productRepository.AddProduct(product);
        }
        public async Task<Product> UpdateProduct(Product product)
        {
            return await _productRepository.UpdateProduct(product);
        }
        public async Task<bool> DeleteProduct(int id)
        {
            return await _productRepository.DeleteProduct(id);
        }
    }
}
