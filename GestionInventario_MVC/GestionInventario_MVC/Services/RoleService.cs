using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionInventario_MVC.DTOs;
using GestionInventario_MVC.Services.Interfaces;

namespace GestionInventario_MVC.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<List<RoleDto>> GetAllAsync()
        {
            return _roleManager.Roles
                .Select(r => new RoleDto { Id = r.Id, Name = r.Name })
                .ToList();
        }

        public async Task<RoleDto?> GetByIdAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return null;
            return new RoleDto { Id = role.Id, Name = role.Name };
        }

        public async Task<bool> CreateAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName)) return false;
            if (await _roleManager.RoleExistsAsync(roleName)) return false;

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            return result.Succeeded;
        }

        public async Task<bool> UpdateAsync(string id, string newRoleName)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return false;

            if (await _roleManager.RoleExistsAsync(newRoleName)) return false;

            role.Name = newRoleName;
            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return false;

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }
    }
}
