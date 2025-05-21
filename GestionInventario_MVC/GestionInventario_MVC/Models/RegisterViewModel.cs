using System.ComponentModel.DataAnnotations;

namespace GestionInventario_MVC.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El campo Nombre Completo es obligatorio.")] // Asumiendo que quieres un FullName
        [Display(Name = "Full Name")]
        public string FullName { get; set; } // Añadido para mapear a AspNetUsers.FullName

        [Required(ErrorMessage = "El campo Email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El campo Email no es una dirección de correo electrónico válida.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo Contraseña es obligatorio.")]
        [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} y un máximo de {1} caracteres de longitud.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }
    }
}