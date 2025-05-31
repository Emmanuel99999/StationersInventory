using GestionInventario_MVC.Areas.Identity.Data; // Para GestionInventario_MVCUser
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore; // Necesario para ToListAsync en GetRolesAsync si consultas directamente
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionInventario_MVC.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<GestionInventario_MVCUser> _userManager; // Para operaciones con usuarios

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<GestionInventario_MVCUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IdentityResult> CreateRoleAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                // Podrías lanzar una ArgumentNullException o devolver un error específico.
                // Por simplicidad, IdentityResult.Failed lo manejará si el nombre es inválido.
            }
            if (await RoleExistsAsync(roleName))
            {
                return IdentityResult.Failed(new IdentityError { Description = $"El rol '{roleName}' ya existe." });
            }
            return await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
        }

        public async Task<IdentityResult> DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Rol con ID '{roleId}' no encontrado." });
            }
            return await _roleManager.DeleteAsync(role);
        }

        public async Task<IdentityRole?> GetRoleByIdAsync(string roleId)
        {
            return await _roleManager.FindByIdAsync(roleId);
        }

        public async Task<IdentityRole?> GetRoleByNameAsync(string roleName)
        {
            return await _roleManager.FindByNameAsync(roleName);
        }

        public async Task<IEnumerable<IdentityRole>> GetRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync(); // Obtiene todos los roles
        }

        public async Task<IdentityResult> UpdateRoleAsync(string roleId, string newRoleName)
        {
            if (string.IsNullOrWhiteSpace(newRoleName))
            {
                // Manejar error de nombre vacío
            }
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Rol con ID '{roleId}' no encontrado." });
            }

            // Verificar si el nuevo nombre de rol ya existe (y no es el mismo rol)
            var existingRoleWithNewName = await _roleManager.FindByNameAsync(newRoleName.Trim());
            if (existingRoleWithNewName != null && existingRoleWithNewName.Id != roleId)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"El nombre de rol '{newRoleName}' ya está en uso." });
            }

            role.Name = newRoleName.Trim();
            // Si tuvieras una clase AppRole con más propiedades:
            // if (role is AppRole appRole && !string.IsNullOrWhiteSpace(newDescription)) { appRole.Description = newDescription; }
            role.NormalizedName = _roleManager.NormalizeKey(newRoleName.Trim()); // Importante actualizar el nombre normalizado

            return await _roleManager.UpdateAsync(role);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName.Trim());
        }

        // --- Métodos de asignación de roles a usuarios ---
        public async Task<IdentityResult> AddUserToRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Usuario con ID '{userId}' no encontrado." });
            }
            if (!await RoleExistsAsync(roleName))
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Rol '{roleName}' no encontrado." });
            }
            return await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> AddUserToRolesAsync(string userId, IEnumerable<string> roleNames)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Usuario con ID '{userId}' no encontrado." });
            }
            foreach (var roleName in roleNames)
            {
                if (!await RoleExistsAsync(roleName))
                {
                    return IdentityResult.Failed(new IdentityError { Description = $"Rol '{roleName}' no encontrado." });
                }
            }
            return await _userManager.AddToRolesAsync(user, roleNames);
        }

        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // Considera cómo manejar esto: lanzar excepción o devolver lista vacía.
                // Por ahora, devolvemos lista vacía para evitar nulls en el código que llama.
                return new List<string>();
            }
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> RemoveUserFromRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Usuario con ID '{userId}' no encontrado." });
            }
            if (!await RoleExistsAsync(roleName))
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Rol '{roleName}' no encontrado." });
            }
            return await _userManager.RemoveFromRoleAsync(user, roleName);
        }
        public async Task<IdentityResult> RemoveUserFromRolesAsync(string userId, IEnumerable<string> roleNames)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Usuario con ID '{userId}' no encontrado." });
            }
            foreach (var roleName in roleNames)
            {
                if (!await RoleExistsAsync(roleName))
                {
                    // Podrías decidir continuar y solo remover los que existen,
                    // o fallar si alguno no existe. Por ahora, fallamos si uno no se encuentra.
                    return IdentityResult.Failed(new IdentityError { Description = $"Rol '{roleName}' no encontrado." });
                }
            }
            return await _userManager.RemoveFromRolesAsync(user, roleNames);
        }
    }
}