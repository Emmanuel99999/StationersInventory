namespace GestionInventario_MVC.Models
{
    public class Venta
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
    }

    public class Compra
    {
        public int Id { get; set; }
        public string Proveedor { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
    }

    public class InventarioItem
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public string Categoria { get; set; }
    }
}