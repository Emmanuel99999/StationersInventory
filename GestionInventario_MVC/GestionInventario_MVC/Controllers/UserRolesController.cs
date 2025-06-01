using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using GestionInventario_MVC.Areas.Identity.Data;

[Authorize(Roles = "Administrador")]
public class UserRolesController : Controller
{
    private readonly UserManager<GestionInventario_MVCUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserRolesController(UserManager<GestionInventario_MVCUser> userManager,
                           RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // GET: Usuarios y roles
    public IActionResult Assign()
    {
        var users = _userManager.Users.ToList();
        var roles = _roleManager.Roles.ToList();

        var userRolesDict = new Dictionary<string, IList<string>>();
        foreach (var user in users)
        {
            var userRoles = _userManager.GetRolesAsync(user).Result;
            userRolesDict[user.Id] = userRoles;
        }

        ViewBag.Roles = roles;
        ViewBag.UserRoles = userRolesDict;
        return View(users);
    }

    // POST: Asignar rol a usuario
    [HttpPost]
    public async Task<IActionResult> Assign(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        // Obtiene TODOS los roles actuales del usuario
        var currentRoles = await _userManager.GetRolesAsync(user);

        // Elimina todos los roles actuales
        if (currentRoles.Any())
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

        // Asigna el nuevo rol seleccionado
        if (!await _userManager.IsInRoleAsync(user, roleName))
            await _userManager.AddToRoleAsync(user, roleName);

        return RedirectToAction("Assign");
    }
    // GET: Confirmar quitar rol
    public async Task<IActionResult> Remove(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        // Comprobar si tiene ese rol
        if (!await _userManager.IsInRoleAsync(user, roleName))
            return NotFound();

        ViewBag.UserId = userId;
        ViewBag.UserName = user.UserName;
        ViewBag.RoleName = roleName;
        return View();
    }

    // POST: Quitar rol confirmado
    [HttpPost]
    public async Task<IActionResult> RemoveConfirmed(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        if (await _userManager.IsInRoleAsync(user, roleName))
            await _userManager.RemoveFromRoleAsync(user, roleName);

        return RedirectToAction("Assign");
    }
}