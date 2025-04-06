using System.Collections.Generic;
using System.Threading.Tasks;
using GestionInventario_MVC.Models;

namespace GestionInventario_MVC.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task<IEnumerable<InventoryMovement>> GetInventoryMovementsAsync(int productId);
    }
}
