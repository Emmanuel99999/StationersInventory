using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GestionInventario_MVC.Models;
using GestionInventario_MVC.Data; // Ajusta según tu proyecto
using Microsoft.EntityFrameworkCore;

namespace GestionInventario_MVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class InventoryMovementApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InventoryMovementApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/InventoryMovement
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryMovement>>> GetInventoryMovements()
        {
            return await _context.InventoryMovements.ToListAsync();
        }

        // GET: api/InventoryMovement/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryMovement>> GetInventoryMovement(int id)
        {
            var movement = await _context.InventoryMovements.FindAsync(id);

            if (movement == null)
                return NotFound();

            return movement;
        }

        // POST: api/InventoryMovement
        [HttpPost]
        public async Task<ActionResult<InventoryMovement>> PostInventoryMovement(InventoryMovement movement)
        {
            _context.InventoryMovements.Add(movement);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetInventoryMovement), new { id = movement.Id }, movement);
        }

        // PUT: api/InventoryMovement/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventoryMovement(int id, InventoryMovement movement)
        {
            if (id != movement.Id)
                return BadRequest();

            _context.Entry(movement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryMovementExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/InventoryMovement/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventoryMovement(int id)
        {
            var movement = await _context.InventoryMovements.FindAsync(id);
            if (movement == null)
                return NotFound();

            _context.InventoryMovements.Remove(movement);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InventoryMovementExists(int id)
        {
            return _context.InventoryMovements.Any(e => e.Id == id);
        }
    }
}