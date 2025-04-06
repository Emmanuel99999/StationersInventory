using System.ComponentModel.DataAnnotations; // Necesario para los atributos

namespace GestionInventario_MVC.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        public string Category { get; set; } = string.Empty;

        // --- VALIDACIÓN PARA STOCK
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser un número negativo.")]
        public int Stock { get; set; }
        

        public string Barcode { get; set; } = string.Empty;
    }
}