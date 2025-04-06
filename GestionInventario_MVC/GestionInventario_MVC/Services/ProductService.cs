using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionInventario_MVC.Models;

namespace GestionInventario_MVC.Services
{
    public class ProductService : IProductService
    {
        private readonly List<Product> _products = new();
        private readonly List<InventoryMovement> _movements = new();

        public Task<IEnumerable<Product>> GetProductsAsync()
        {
            return Task.FromResult(_products.AsEnumerable());
        }

        public Task<Product> GetProductByIdAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(product ?? throw new KeyNotFoundException($"Product with ID {id} not found."));
        }


        public Task AddProductAsync(Product product)
        {
            _products.Add(product);
            return Task.CompletedTask;
        }

        public Task UpdateProductAsync(Product product)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Category = product.Category;
                existingProduct.Stock = product.Stock;
                existingProduct.Barcode = product.Barcode;
            }
            return Task.CompletedTask;
        }

        public Task DeleteProductAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<InventoryMovement>> GetInventoryMovementsAsync(int productId)
        {
            return Task.FromResult(_movements.Where(m => m.ProductId == productId).AsEnumerable());
        }
    }
}