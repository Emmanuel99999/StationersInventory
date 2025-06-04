using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestionInventario_MVC.Models;
using System.Collections.Generic;
using System.Linq;

namespace GestionInventario_MVC.Controllers
{
    [Authorize]
    public class EstadisticasController : Controller
    {
        public IActionResult GraficosTendencia()
        {
            // Datos simulados: ventas mensuales de productos de papelería
            var datos = new List<TendenciaVentaMensual>
            {
                new TendenciaVentaMensual { Mes = "Enero", Ventas = 120 },
                new TendenciaVentaMensual { Mes = "Febrero", Ventas = 135 },
                new TendenciaVentaMensual { Mes = "Marzo", Ventas = 150 },
                new TendenciaVentaMensual { Mes = "Abril", Ventas = 110 },
                new TendenciaVentaMensual { Mes = "Mayo", Ventas = 180 },
                new TendenciaVentaMensual { Mes = "Junio", Ventas = 170 },
                new TendenciaVentaMensual { Mes = "Julio", Ventas = 190 },
                new TendenciaVentaMensual { Mes = "Agosto", Ventas = 200 },
                new TendenciaVentaMensual { Mes = "Septiembre", Ventas = 160 },
                new TendenciaVentaMensual { Mes = "Octubre", Ventas = 175 },
                new TendenciaVentaMensual { Mes = "Noviembre", Ventas = 210 },
                new TendenciaVentaMensual { Mes = "Diciembre", Ventas = 250 }
            };

            // Ejemplo de estadísticas simples:
            ViewBag.TotalVentasAnio = datos.Sum(d => d.Ventas);
            ViewBag.PromedioMensual = datos.Average(d => d.Ventas);
            ViewBag.MaximoMes = datos.OrderByDescending(d => d.Ventas).First().Mes;
            ViewBag.MaximoMesVentas = datos.Max(d => d.Ventas);

            return View(datos);
        }
    }
}