namespace GestionInventario_MVC.Models
{
    public class ProductoMasVendido
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int CantidadVendida { get; set; }
        public string Categoria { get; set; }
    }
}