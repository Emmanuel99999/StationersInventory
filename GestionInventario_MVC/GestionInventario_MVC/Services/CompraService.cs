using GestionInventario_MVC.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GestionInventario_MVC.Services
{
    public class CompraService : ICompraService
    {
        private readonly List<OrdenCompra> _ordenesCompra = new();
        private readonly List<ControlCredito> _controlCreditos = new();
        private readonly List<PagoProveedor> _pagosProveedores = new();
        private readonly List<HistorialCompra> _historialCompras = new();
        private readonly List<RecepcionMercancia> _recepcionMercancias = new();
        private readonly List<Proveedor> _proveedores = new();

        public Task<IEnumerable<OrdenCompra>> GetOrdenesCompraAsync() => Task.FromResult(_ordenesCompra.AsEnumerable());
        public Task AddOrdenCompraAsync(OrdenCompra ordenCompra)
        {
            Console.WriteLine($"DEBUG - Fecha recibida en AddOrdenCompraAsync: {ordenCompra.FechaOrden}");
            ordenCompra.Id = _ordenesCompra.Count + 1;
            _ordenesCompra.Add(ordenCompra);
            return Task.CompletedTask;
        }
        public Task UpdateOrdenCompraAsync(OrdenCompra ordenCompra)
        {
            var existing = _ordenesCompra.FirstOrDefault(o => o.Id == ordenCompra.Id);
            if (existing != null)
            {
                existing.ProveedorId = ordenCompra.ProveedorId;
                existing.FechaOrden = ordenCompra.FechaOrden;
            }
            return Task.CompletedTask;
        }
        public Task DeleteOrdenCompraAsync(int id)
        {
            _ordenesCompra.RemoveAll(o => o.Id == id);
            return Task.CompletedTask;
        }

        // Control de Crédito
        public Task<IEnumerable<ControlCredito>> GetControlCreditosAsync() => Task.FromResult(_controlCreditos.AsEnumerable());
        public Task UpdateControlCreditoAsync(ControlCredito controlCredito)
        {
            var existing = _controlCreditos.FirstOrDefault(c => c.Id == controlCredito.Id);
            if (existing != null)
            {
                existing.CreditoDisponible = controlCredito.CreditoDisponible;
                existing.FechaVencimiento = controlCredito.FechaVencimiento;
            }
            return Task.CompletedTask;
        }


        // Pagos a Proveedores
        public Task<IEnumerable<PagoProveedor>> GetPagosProveedoresAsync() => Task.FromResult(_pagosProveedores.AsEnumerable());
        public Task AddPagoProveedorAsync(PagoProveedor pagoProveedor)
        {
            pagoProveedor.Id = _pagosProveedores.Count + 1;
            _pagosProveedores.Add(pagoProveedor);
            return Task.CompletedTask;
        }
        public Task UpdatePagoProveedorAsync(PagoProveedor pagoProveedor)
        {
            var existing = _pagosProveedores.FirstOrDefault(p => p.Id == pagoProveedor.Id);
            if (existing != null)
            {
                existing.ProveedorId = pagoProveedor.ProveedorId;
                existing.Monto = pagoProveedor.Monto;
                existing.FechaPago = pagoProveedor.FechaPago;
            }
            return Task.CompletedTask;
        }

        public Task DeletePagoProveedorAsync(int id)
        {
            _pagosProveedores.RemoveAll(p => p.Id == id);
            return Task.CompletedTask;
        }


        // Historial de Compras
        public Task<IEnumerable<HistorialCompra>> GetHistorialComprasAsync() => Task.FromResult(_historialCompras.AsEnumerable());

        // Recepción de Mercancía
        public Task<IEnumerable<RecepcionMercancia>> GetRecepcionMercanciasAsync() => Task.FromResult(_recepcionMercancias.AsEnumerable());
        public Task AddRecepcionMercanciaAsync(RecepcionMercancia recepcionMercancia)
        {
            recepcionMercancia.Id = _recepcionMercancias.Count + 1;
            _recepcionMercancias.Add(recepcionMercancia);
            return Task.CompletedTask;
        }
        public Task AddHistorialCompraAsync(HistorialCompra historialCompra)
        {
            historialCompra.Id = _historialCompras.Count + 1;
            _historialCompras.Add(historialCompra);
            return Task.CompletedTask;
        }
        public Task UpdateRecepcionMercanciaAsync(RecepcionMercancia recepcionMercancia)
        {
            var existing = _recepcionMercancias.FirstOrDefault(r => r.Id == recepcionMercancia.Id);
            if (existing != null)
            {
                existing.OrdenCompraId = recepcionMercancia.OrdenCompraId;
                existing.FechaRecepcion = recepcionMercancia.FechaRecepcion;
                existing.Productos = recepcionMercancia.Productos;
            }
            return Task.CompletedTask;
        }

        public Task DeleteRecepcionMercanciaAsync(int id)
        {
            _recepcionMercancias.RemoveAll(r => r.Id == id);
            return Task.CompletedTask;
        }


        public Task UpdateHistorialCompraAsync(HistorialCompra historialCompra)
        {
            var existing = _historialCompras.FirstOrDefault(h => h.Id == historialCompra.Id);
            if (existing != null)
            {
                existing.ProveedorId = historialCompra.ProveedorId;
                existing.FechaCompra = historialCompra.FechaCompra;
                existing.Productos = historialCompra.Productos;
            }
            return Task.CompletedTask;
        }

        public Task DeleteHistorialCompraAsync(int id)
        {
            _historialCompras.RemoveAll(h => h.Id == id);
            return Task.CompletedTask;
        }

        // Gestión de Proveedores
        public Task<IEnumerable<Proveedor>> GetProveedoresAsync() => Task.FromResult(_proveedores.AsEnumerable());
        public Task AddProveedorAsync(Proveedor proveedor)
        {
            proveedor.Id = _proveedores.Count + 1;
            _proveedores.Add(proveedor);
            return Task.CompletedTask;
        }
        public Task UpdateProveedorAsync(Proveedor proveedor)
        {
            var existing = _proveedores.FirstOrDefault(p => p.Id == proveedor.Id);
            if (existing != null)
            {
                existing.Nombre = proveedor.Nombre;
                existing.Direccion = proveedor.Direccion;
                existing.Telefono = proveedor.Telefono;
                existing.Email = proveedor.Email;
            }
            return Task.CompletedTask;
        }
        public Task DeleteProveedorAsync(int id)
        {
            _proveedores.RemoveAll(p => p.Id == id);
            return Task.CompletedTask;
        }

        
        public Task AddControlCreditoAsync(ControlCredito controlCredito)
        {
            controlCredito.Id = _controlCreditos.Count + 1;
            _controlCreditos.Add(controlCredito);
            return Task.CompletedTask;
        }

        public Task DeleteControlCreditoAsync(int id)
        {
            _controlCreditos.RemoveAll(c => c.Id == id);
            return Task.CompletedTask;
        }

    }

}
