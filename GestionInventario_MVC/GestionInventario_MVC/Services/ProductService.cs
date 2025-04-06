using System;
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
        private int _nextProductId = 1;

        public Task<IEnumerable<Product>> GetProductsAsync()
        {
            return Task.FromResult(_products.AsEnumerable());
        }

        // --- INICIO: CORRECCIÓN NULLABILITY ---
        public Task<Product?> GetProductByIdAsync(int id) // Cambiado a Product?
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(product); // Ahora es seguro devolver product (que puede ser null)
        }
        // --- FIN: CORRECCIÓN NULLABILITY ---

        public Task AddProductAsync(Product product)
        {
            if (!string.IsNullOrWhiteSpace(product.Barcode) &&
                _products.Any(p => p.Barcode.Equals(product.Barcode, StringComparison.OrdinalIgnoreCase)))
            {
                throw new DuplicateProductException($"Ya existe un producto con el código de barras '{product.Barcode}'.");
            }
            if (_products.Any(p => p.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new DuplicateProductException($"Ya existe un producto con el nombre '{product.Name}'.");
            }

            product.Id = _nextProductId++;
            _products.Add(product);

            _movements.Add(new InventoryMovement
            {
                Date = DateTime.Now,
                ProductId = product.Id,
                Quantity = product.Stock,
                Type = "Entrada Inicial"
            });
            return Task.CompletedTask;
        }

        public Task UpdateProductAsync(Product product)
        {
            if (!string.IsNullOrWhiteSpace(product.Barcode) &&
                _products.Any(p => p.Id != product.Id && p.Barcode.Equals(product.Barcode, StringComparison.OrdinalIgnoreCase)))
            {
                throw new DuplicateProductException($"Ya existe OTRO producto con el código de barras '{product.Barcode}'.");
            }
            if (_products.Any(p => p.Id != product.Id && p.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new DuplicateProductException($"Ya existe OTRO producto con el nombre '{product.Name}'.");
            }

            var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                if (product.Stock != existingProduct.Stock)
                {
                    _movements.Add(new InventoryMovement
                    {
                        Date = DateTime.Now,
                        ProductId = product.Id,
                        Quantity = product.Stock - existingProduct.Stock,
                        Type = (product.Stock > existingProduct.Stock) ? "Ajuste Entrada" : "Ajuste Salida"
                    });
                }
                existingProduct.Name = product.Name;
                existingProduct.Category = product.Category;
                existingProduct.Stock = product.Stock;
                existingProduct.Barcode = product.Barcode;
            }
            else
            {
                Console.WriteLine($"ADVERTENCIA: Se intentó actualizar producto con ID {product.Id} pero no se encontró.");
                // Opcional: throw new KeyNotFoundException($"No se encontró producto con ID {product.Id} para actualizar.");
            }
            return Task.CompletedTask;
        }

        public Task DeleteProductAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _movements.Add(new InventoryMovement
                {
                    Date = DateTime.Now,
                    ProductId = id,
                    Quantity = -product.Stock,
                    Type = "Eliminación"
                });
                _products.Remove(product);
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<InventoryMovement>> GetInventoryMovementsAsync(int productId = 0)
        {
            if (productId > 0)
            {
                return Task.FromResult(_movements.Where(m => m.ProductId == productId).AsEnumerable());
            }
            else
            {
                return Task.FromResult(_movements.AsEnumerable());
            }
        }
    }
}