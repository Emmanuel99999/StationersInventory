using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GestionInventario_MVC.Models;
using System.Collections.Generic;

namespace GestionInventario_MVC.Controllers
{
    [Authorize]
    public class AnalisisController : Controller
    {
        public IActionResult ProductosMasVendidos()
        {
            // Datos simulados de papelería
            var productos = new List<ProductoMasVendido>
            {
                new ProductoMasVendido { Id = 1, Nombre = "Bolígrafo Azul", Categoria = "Escritura", CantidadVendida = 340 },
                new ProductoMasVendido { Id = 2, Nombre = "Cuaderno Profesional", Categoria = "Papelería", CantidadVendida = 210 },
                new ProductoMasVendido { Id = 3, Nombre = "Resaltador Amarillo", Categoria = "Escritura", CantidadVendida = 160 },
                new ProductoMasVendido { Id = 4, Nombre = "Lápiz HB", Categoria = "Escritura", CantidadVendida = 120 },
                new ProductoMasVendido { Id = 5, Nombre = "Carpeta Plástica", Categoria = "Organización", CantidadVendida = 95 }
            };

            return View(productos);
        }
    }
}