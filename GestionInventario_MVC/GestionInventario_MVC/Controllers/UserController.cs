using GestionInventario_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using GestionInventario_MVC.Areas.Identity.Data; // Importante para tu clase de usuario GestionInventario_MVCUser
using Microsoft.AspNetCore.Authorization;
using GestionInventario_MVC.Data; // Importante para tu DbContext AppDbContext
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient; // Necesario para SqlParameter

namespace GestionInventario_MVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<GestionInventario_MVCUser> _userManager;
        private readonly SignInManager<GestionInventario_MVCUser> _signInManager;
        private readonly AppDbContext _context; // ¡Cambiado a AppDbContext!

        public UsersController(UserManager<GestionInventario_MVCUser> userManager,
                               SignInManager<GestionInventario_MVCUser> signInManager,
                               AppDbContext context) // ¡Cambiado a AppDbContext!
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Crear un nuevo usuario usando GestionInventario_MVCUser
            // Asegúrate de que RegisterViewModel tenga una propiedad FullName
            var user = new GestionInventario_MVCUser { UserName = model.Email, Email = model.Email, FullName = model.FullName };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var userId = user.Id;

                // Llama al procedimiento almacenado para crear datos por defecto
                // Asegúrate de que el nombre del parámetro coincida con el SP
                await _context.Database.ExecuteSqlRawAsync("EXEC CrearDatosUsuarioNuevo @UsuarioId",
                                                            new SqlParameter("UsuarioId", userId));

                await _signInManager.SignInAsync(user, isPersistent: true);

                return RedirectToAction("Index", "Transacciones");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // lockoutOnFailure: false significa que la cuenta no se bloqueará
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Transacciones");
            }

            ModelState.AddModelError(string.Empty, "Nombre de usuario o password incorrecto.");

            return View(model);
        }
    }
}