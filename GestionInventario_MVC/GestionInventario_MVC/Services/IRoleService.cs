using System.Collections.Generic;
using System.Threading.Tasks;
using GestionInventario_MVC.DTOs;

namespace GestionInventario_MVC.Services.Interfaces
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetAllAsync();
        Task<RoleDto?> GetByIdAsync(string id);
        Task<bool> CreateAsync(string roleName);
        Task<bool> UpdateAsync(string id, string newRoleName);
        Task<bool> DeleteAsync(string id);
    }
}
