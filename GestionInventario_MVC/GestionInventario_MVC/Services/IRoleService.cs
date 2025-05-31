using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionInventario_MVC.Services
{
	public interface IRoleService
	{
		Task<IEnumerable<IdentityRole>> GetRolesAsync();
		Task<IdentityRole?> GetRoleByIdAsync(string roleId);
		Task<IdentityRole?> GetRoleByNameAsync(string roleName);
		Task<IdentityResult> CreateRoleAsync(string roleName);
		Task<IdentityResult> UpdateRoleAsync(string roleId, string newRoleName);
		Task<IdentityResult> DeleteRoleAsync(string roleId);
		Task<bool> RoleExistsAsync(string roleName);

		// Métodos para asignar/desasignar roles a usuarios (pueden estar aquí o en un IUserService)
		// Backend A.
		// La Persona 3 (Backend B - Usuarios) también necesitará interactuar con esto.
		Task<IdentityResult> AddUserToRoleAsync(string userId, string roleName);
		Task<IdentityResult> AddUserToRolesAsync(string userId, IEnumerable<string> roleNames);
		Task<IdentityResult> RemoveUserFromRoleAsync(string userId, string roleName);
		Task<IdentityResult> RemoveUserFromRolesAsync(string userId, IEnumerable<string> roleNames);
		Task<IList<string>> GetUserRolesAsync(string userId);
	}
}