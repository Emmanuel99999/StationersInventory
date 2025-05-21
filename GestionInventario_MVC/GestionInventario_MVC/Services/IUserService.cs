using GestionInventario_MVC.Areas.Identity.Data; // Para GestionInventario_MVCUser si necesitas referenciarlo directamente
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionInventario_MVC.Services
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IList<string> Roles { get; set; } = new List<string>();
        // public string FullName { get; set; } = string.Empty; // Descomenta si tienes FullName y quieres usarlo
    }

    public class CreateUserDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
        // public string FullName { get; set; } = string.Empty; // Descomenta si tienes FullName
    }

    public class UpdateUserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
        public string? NewPassword { get; set; } // Para el cambio opcional de contraseña
        // public string FullName { get; set; } = string.Empty; // Descomenta si tienes FullName
    }

    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUsersWithRolesAsync();
        Task<UserDto?> GetUserWithRolesByIdAsync(string userId);
        Task<(IdentityResult result, string? userId)> CreateUserAsync(CreateUserDto userDto);
        Task<IdentityResult> UpdateUserAsync(UpdateUserDto userDto);
        Task<IdentityResult> DeleteUserAsync(string userId);
    }
}