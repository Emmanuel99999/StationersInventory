using GestionInventario_MVC.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GestionInventario_MVC.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<InventoryMovement> InventoryMovements { get; set; } 
    }
}
