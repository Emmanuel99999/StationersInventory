using GestionInventario_MVC.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GestionInventario_MVC.Services
{
    public interface ICompraService
    {
        // Órdenes de Compra
        Task<IEnumerable<OrdenCompra>> GetOrdenesCompraAsync();
        Task AddOrdenCompraAsync(OrdenCompra ordenCompra);
        Task UpdateOrdenCompraAsync(OrdenCompra ordenCompra);
        Task DeleteOrdenCompraAsync(int id);

        // Control de Crédito
        Task<IEnumerable<ControlCredito>> GetControlCreditosAsync();
        Task UpdateControlCreditoAsync(ControlCredito controlCredito);

        // Pagos a Proveedores
        Task<IEnumerable<PagoProveedor>> GetPagosProveedoresAsync();
        Task AddPagoProveedorAsync(PagoProveedor pagoProveedor);
        Task UpdatePagoProveedorAsync(PagoProveedor pagoProveedor);
        Task DeletePagoProveedorAsync(int id);


        // Historial de Compras
        Task<IEnumerable<HistorialCompra>> GetHistorialComprasAsync();
        Task AddHistorialCompraAsync(HistorialCompra historialCompra);
        Task UpdateHistorialCompraAsync(HistorialCompra historialCompra);
        Task DeleteHistorialCompraAsync(int id);

        // Recepción de Mercancía
        Task<IEnumerable<RecepcionMercancia>> GetRecepcionMercanciasAsync();
        Task AddRecepcionMercanciaAsync(RecepcionMercancia recepcionMercancia);
        Task DeleteRecepcionMercanciaAsync(int id);
        Task UpdateRecepcionMercanciaAsync(RecepcionMercancia recepcionMercancia);


        // Gestión de Proveedores
        Task<IEnumerable<Proveedor>> GetProveedoresAsync();
        Task AddProveedorAsync(Proveedor proveedor);
        Task UpdateProveedorAsync(Proveedor proveedor);
        Task DeleteProveedorAsync(int id);

        // Control de credito
        Task AddControlCreditoAsync(ControlCredito controlCredito);
        Task DeleteControlCreditoAsync(int id);

    }

}
