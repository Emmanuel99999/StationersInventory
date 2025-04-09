using System.ComponentModel.DataAnnotations;

namespace GestionInventario_MVC.Models
{
    // Models/Proveedor.cs
    public class Proveedor
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre del proveedor es obligatorio.")]
        public string Nombre { get; set; } = string.Empty;
        [Required(ErrorMessage = "La dirección es obligatoria.")]
        public string Direccion { get; set; } = string.Empty;
        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [Phone(ErrorMessage = "El número de teléfono no tiene un formato válido.")]
        public string Telefono { get; set; } = string.Empty;
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        public string Email { get; set; } = string.Empty;
    }

    /// clases para implementar a mayor profundidad luego del primer reporte
    // Models/Producto.cs
    public class Producto
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
    public decimal Precio { get; set; }
    public int Cantidad { get; set; }
}


// Models/OrdenCompra.cs
public class OrdenCompra
{
    public int Id { get; set; }
    public int ProveedorId { get; set; }
    public DateTime FechaOrden { get; set; }
    public required List<Producto> Productos { get; set; }
}

// Models/RecepcionMercancia.cs
public class RecepcionMercancia
{
    public int Id { get; set; }
    public int OrdenCompraId { get; set; }
    public DateTime FechaRecepcion { get; set; }
    public required List<Producto> Productos { get; set; }
}

// Models/HistorialCompra.cs
public class HistorialCompra
{
    public int Id { get; set; }
    public int ProveedorId { get; set; }
    public DateTime FechaCompra { get; set; }
    public required List<Producto> Productos { get; set; }
}

// Models/PagoProveedor.cs
public class PagoProveedor
{
    public int Id { get; set; }
    public int ProveedorId { get; set; }
    public decimal Monto { get; set; }
    public DateTime FechaPago { get; set; }
}

// Models/ControlCredito.cs

    public class ControlCredito
    {
        public int Id { get; set; }
        public int ProveedorId { get; set; }
        public decimal CreditoDisponible { get; set; }
        public DateTime FechaVencimiento { get; set; }
    }


}