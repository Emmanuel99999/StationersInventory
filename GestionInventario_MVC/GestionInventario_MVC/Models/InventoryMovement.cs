namespace GestionInventario_MVC.Models
{
    public class InventoryMovement
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; } = string.Empty; // Initialize with a default value
    }

}