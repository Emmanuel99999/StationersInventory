using System.ComponentModel.DataAnnotations;

public class EditProfileViewModel
{
    public string Email { get; set; } // Solo para mostrar, no editable por ahora

    [Required]
    [Display(Name = "Nombre completo")]
    public string FullName { get; set; }

    [Phone]
    [Display(Name = "Teléfono")]
    public string PhoneNumber { get; set; }
}
