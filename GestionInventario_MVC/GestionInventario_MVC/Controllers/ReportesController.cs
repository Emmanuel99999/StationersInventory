using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using GestionInventario_MVC.Models;

namespace GestionInventario_MVC.Controllers
{
    [Authorize]
    public class ReportesController : Controller
    {
        // REPORTE DE VENTAS
        public IActionResult Ventas()
        {
            var ventas = new List<Venta>
            {
                new Venta { Id = 1, Cliente = "Juan Pérez", Fecha = DateTime.Today.AddDays(-2), Total = 1500 },
                new Venta { Id = 2, Cliente = "Ana López", Fecha = DateTime.Today.AddDays(-1), Total = 2450 },
                new Venta { Id = 3, Cliente = "Carlos Ruiz", Fecha = DateTime.Today, Total = 980 }
            };
            return View(ventas);
        }

        // REPORTE DE COMPRAS
        public IActionResult Compras()
        {
            var compras = new List<Compra>
            {
                new Compra { Id = 1, Proveedor = "Proveedor A", Fecha = DateTime.Today.AddDays(-5), Total = 3000 },
                new Compra { Id = 2, Proveedor = "Proveedor B", Fecha = DateTime.Today.AddDays(-1), Total = 1750 },
                new Compra { Id = 3, Proveedor = "Proveedor C", Fecha = DateTime.Today, Total = 2200 }
            };
            return View(compras);
        }

        // REPORTE DE INVENTARIO
        public IActionResult Inventario()
        {
            var inventario = new List<InventarioItem>
            {
                new InventarioItem { Id = 1, Nombre = "Lapiz rojo", Cantidad = 50, Categoria = "lapiz" },
                new InventarioItem { Id = 2, Nombre = "Cuaderno", Cantidad = 20, Categoria = "cuadernos" },
                new InventarioItem { Id = 3, Nombre = "Borrador", Cantidad = 80, Categoria = "accesorios" }
            };
            return View(inventario);
        }
    }
}