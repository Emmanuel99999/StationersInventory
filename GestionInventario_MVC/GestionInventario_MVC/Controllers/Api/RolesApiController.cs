using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GestionInventario_MVC.Services.Interfaces;
using GestionInventario_MVC.DTOs;

namespace GestionInventario_MVC.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrador")]
    public class RolesApiController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesApiController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // GET: api/roles
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }

        // GET: api/roles/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var role = await _roleService.GetByIdAsync(id);
            if (role == null)
                return NotFound(new { message = "Rol no encontrado" });

            return Ok(role);
        }

        // POST: api/roles
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleDto model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
                return BadRequest(new { message = "El nombre del rol es obligatorio" });

            var success = await _roleService.CreateAsync(model.Name);
            if (!success)
                return Conflict(new { message = "El rol ya existe o no se pudo crear" });

            return Ok(new { message = "Rol creado correctamente" });
        }

        // PUT: api/roles/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] RoleDto model)
        {
            var success = await _roleService.UpdateAsync(id, model.Name);
            if (!success)
                return BadRequest(new { message = "No se pudo actualizar el rol" });

            return Ok(new { message = "Rol actualizado correctamente" });
        }

        // DELETE: api/roles/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _roleService.DeleteAsync(id);
            if (!success)
                return NotFound(new { message = "No se pudo eliminar el rol" });

            return Ok(new { message = "Rol eliminado correctamente" });
        }
    }
}
