namespace GestionInventario_MVC.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Initialize with a default value
        public string Category { get; set; } = string.Empty; // Initialize with a default value
        public int Stock { get; set; }
        public string Barcode { get; set; } = string.Empty; // Initialize with a default value
    }

}

