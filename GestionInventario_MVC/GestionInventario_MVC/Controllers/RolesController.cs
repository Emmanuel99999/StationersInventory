using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Authorize(Roles = "Administrador")]
public class RolesController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RolesController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    // Listar roles
    public IActionResult Index() => View(_roleManager.Roles);

    // Crear rol (GET)
    public IActionResult Create() => View();

    // Crear rol (POST)
    [HttpPost]
    public async Task<IActionResult> Create(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            ModelState.AddModelError("", "El nombre del rol es obligatorio");
            return View();
        }

        if (await _roleManager.RoleExistsAsync(roleName))
        {
            ModelState.AddModelError("", "El rol ya existe");
            return View();
        }

        await _roleManager.CreateAsync(new IdentityRole(roleName));
        return RedirectToAction(nameof(Index));
    }

    // Editar rol (GET)
    public async Task<IActionResult> Edit(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null) return NotFound();
        return View(role);
    }

    // Editar rol (POST)
    [HttpPost]
    public async Task<IActionResult> Edit(string id, string roleName)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null) return NotFound();

        if (await _roleManager.RoleExistsAsync(roleName))
        {
            ModelState.AddModelError("", "El nombre del rol ya existe");
            return View(role);
        }

        role.Name = roleName;
        await _roleManager.UpdateAsync(role);
        return RedirectToAction(nameof(Index));
    }

    // Eliminar rol (GET)
    public async Task<IActionResult> Delete(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null) return NotFound();
        return View(role);
    }

    // Eliminar rol (POST)
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null) return NotFound();

        await _roleManager.DeleteAsync(role);
        return RedirectToAction(nameof(Index));
    }
}