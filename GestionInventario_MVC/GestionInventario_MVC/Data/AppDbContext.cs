using GestionInventario_MVC.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using GestionInventario_MVC.Areas.Identity.Data;

namespace GestionInventario_MVC.Data
{
    public class AppDbContext : IdentityDbContext<GestionInventario_MVCUser>

    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<InventoryMovement> InventoryMovements { get; set; } 
        public DbSet<Proveedor> Proveedores { get; set; }

    }
}
