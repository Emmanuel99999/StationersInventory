using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionInventario_MVC.Data;
using GestionInventario_MVC.Models;
using Microsoft.EntityFrameworkCore;


namespace GestionInventario_MVC.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }
        // --- FIN: CORRECCIÓN NULLABILITY ---

        public async Task AddProductAsync(Product product)
        {
            if (!string.IsNullOrWhiteSpace(product.Barcode) &&
                await _context.Products.AnyAsync(p => p.Barcode == product.Barcode))
            {
                throw new DuplicateProductException($"Ya existe un producto con el código de barras '{product.Barcode}'.");
            }

            if (await _context.Products.AnyAsync(p => p.Name == product.Name))
            {
                throw new DuplicateProductException($"Ya existe un producto con el nombre '{product.Name}'.");
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var movement = new InventoryMovement
            {
                ProductId = product.Id,
                Quantity = product.Stock,
                Type = "Entrada Inicial",
                Date = DateTime.Now
            };

            _context.InventoryMovements.Add(movement);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            if (!string.IsNullOrWhiteSpace(product.Barcode) &&
                await _context.Products.AnyAsync(p => p.Id != product.Id && p.Barcode == product.Barcode))
            {
                throw new DuplicateProductException($"Ya existe OTRO producto con el código de barras '{product.Barcode}'.");
            }

            if (await _context.Products.AnyAsync(p => p.Id != product.Id && p.Name == product.Name))
            {
                throw new DuplicateProductException($"Ya existe OTRO producto con el nombre '{product.Name}'.");
            }

            var existingProduct = await _context.Products.FindAsync(product.Id);
            if (existingProduct == null) return;

            if (existingProduct.Stock != product.Stock)
            {
                var movement = new InventoryMovement
                {
                    ProductId = product.Id,
                    Quantity = product.Stock - existingProduct.Stock,
                    Type = product.Stock > existingProduct.Stock ? "Ajuste Entrada" : "Ajuste Salida",
                    Date = DateTime.Now
                };
                _context.InventoryMovements.Add(movement);
            }

            existingProduct.Name = product.Name;
            existingProduct.Category = product.Category;
            existingProduct.Stock = product.Stock;
            existingProduct.Barcode = product.Barcode;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return;

            var movement = new InventoryMovement
            {
                ProductId = id,
                Quantity = -product.Stock,
                Type = "Eliminación",
                Date = DateTime.Now
            };
            _context.InventoryMovements.Add(movement);

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<InventoryMovement>> GetInventoryMovementsAsync(int productId = 0)
        {
            if (productId > 0)
                return await _context.InventoryMovements
                    .Where(m => m.ProductId == productId)
                    .ToListAsync();

            return await _context.InventoryMovements.ToListAsync();
        }
    }
}