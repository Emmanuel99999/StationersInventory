using GestionInventario_MVC.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionInventario_MVC.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<GestionInventario_MVCUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserService> _logger;

        public UserService(
            UserManager<GestionInventario_MVCUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<UserService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<(IdentityResult result, string? userId)> CreateUserAsync(CreateUserDto userDto)
        {
            if (userDto == null || string.IsNullOrWhiteSpace(userDto.Email) || string.IsNullOrWhiteSpace(userDto.Password))
            {
                return (IdentityResult.Failed(new IdentityError { Code = "InputError", Description = "Email y contraseña son obligatorios." }), null);
            }

            var existingUserByEmail = await _userManager.FindByEmailAsync(userDto.Email);
            if (existingUserByEmail != null)
            {
                return (IdentityResult.Failed(new IdentityError { Code = "DuplicateEmail", Description = $"El correo electrónico '{userDto.Email}' ya está registrado." }), null);
            }

            var user = new GestionInventario_MVCUser
            {
                UserName = userDto.Email,
                Email = userDto.Email,
                EmailConfirmed = true,
                // FullName = userDto.FullName, // Asegúrate que GestionInventario_MVCUser tenga esta propiedad si la usas
            };

            var result = await _userManager.CreateAsync(user, userDto.Password);
            if (result.Succeeded && userDto.Roles != null && userDto.Roles.Any())
            {
                var rolesToAdd = new List<string>();
                foreach (var roleName in userDto.Roles)
                {
                    if (await _roleManager.RoleExistsAsync(roleName))
                    {
                        rolesToAdd.Add(roleName);
                    }
                    else
                    {
                        _logger.LogWarning($"Rol '{roleName}' no encontrado al intentar crear usuario '{userDto.Email}'. Este rol no será asignado.");
                    }
                }
                if (rolesToAdd.Any())
                {
                    var addToRolesResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                    if (!addToRolesResult.Succeeded)
                    {
                        var allErrors = result.Errors.Concat(addToRolesResult.Errors).ToArray();
                        return (IdentityResult.Failed(allErrors), user.Id);
                    }
                }
            }
            return (result, result.Succeeded ? user.Id : null);
        }

        public async Task<IdentityResult> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Code = "UserNotFound", Description = $"Usuario con ID '{userId}' no encontrado." });
            }
            return await _userManager.DeleteAsync(user);
        }

        public async Task<UserDto?> GetUserWithRolesByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName ?? "",
                Email = user.Email ?? "",
                Roles = await _userManager.GetRolesAsync(user),
                // FullName = user.FullName ?? ""
            };
        }

        public async Task<IEnumerable<UserDto>> GetUsersWithRolesAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDtos = new List<UserDto>();
            foreach (var user in users)
            {
                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName ?? "",
                    Email = user.Email ?? "",
                    Roles = await _userManager.GetRolesAsync(user),
                    // FullName = user.FullName ?? ""
                });
            }
            return userDtos;
        }

        public async Task<IdentityResult> UpdateUserAsync(UpdateUserDto userDto)
        {
            if (userDto == null || string.IsNullOrWhiteSpace(userDto.Id))
            {
                return IdentityResult.Failed(new IdentityError { Code = "InputError", Description = "ID de usuario es obligatorio." });
            }

            var user = await _userManager.FindByIdAsync(userDto.Id);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Code = "UserNotFound", Description = $"Usuario con ID '{userDto.Id}' no encontrado." });
            }

            List<IdentityError> allErrors = new List<IdentityError>();

            // Actualizar Email y UserName
            if (!string.IsNullOrWhiteSpace(userDto.Email) && user.Email != userDto.Email)
            {
                var existingUserWithNewEmail = await _userManager.FindByEmailAsync(userDto.Email);
                if (existingUserWithNewEmail != null && existingUserWithNewEmail.Id != user.Id)
                {
                    allErrors.Add(new IdentityError { Code = "DuplicateEmail", Description = $"El correo electrónico '{userDto.Email}' ya está en uso por otro usuario." });
                }
                else
                {
                    var setEmailResult = await _userManager.SetEmailAsync(user, userDto.Email);
                    if (!setEmailResult.Succeeded) allErrors.AddRange(setEmailResult.Errors);

                    var setUserNameResult = await _userManager.SetUserNameAsync(user, userDto.Email);
                    if (!setUserNameResult.Succeeded) allErrors.AddRange(setUserNameResult.Errors);
                }
            }
            // Actualizar otras propiedades (ej. FullName)
            // if (userDto.FullName != null && user.FullName != userDto.FullName) user.FullName = userDto.FullName;

            // Aplicar cambios de propiedades del usuario
            if (!allErrors.Any())
            {
                var userPropertiesUpdateResult = await _userManager.UpdateAsync(user); // Guarda FullName, Email, UserName, EmailConfirmed, etc.
                if (!userPropertiesUpdateResult.Succeeded) allErrors.AddRange(userPropertiesUpdateResult.Errors);
            }

            // Cambiar contraseña si se proporcionó una nueva y no hubo errores previos
            if (!allErrors.Any() && !string.IsNullOrWhiteSpace(userDto.NewPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetPasswordResult = await _userManager.ResetPasswordAsync(user, token, userDto.NewPassword);
                if (!resetPasswordResult.Succeeded) allErrors.AddRange(resetPasswordResult.Errors);
            }

            // Gestionar roles si no hubo errores previos
            if (!allErrors.Any())
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                var rolesToAssignInDto = userDto.Roles ?? new List<string>();

                var rolesToRemove = currentRoles.Except(rolesToAssignInDto).ToList();
                if (rolesToRemove.Any())
                {
                    var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                    if (!removeRolesResult.Succeeded) allErrors.AddRange(removeRolesResult.Errors);
                }

                var rolesToAdd = rolesToAssignInDto.Except(currentRoles).ToList();
                if (rolesToAdd.Any())
                {
                    var existingRolesToAdd = new List<string>();
                    foreach (var roleName in rolesToAdd)
                    {
                        if (await _roleManager.RoleExistsAsync(roleName)) { existingRolesToAdd.Add(roleName); }
                        else { allErrors.Add(new IdentityError { Code = "RoleNotFoundUpdate", Description = $"El rol '{roleName}' no existe y no se puede asignar." }); }
                    }
                    if (existingRolesToAdd.Any() && !allErrors.Any(e => e.Code == "RoleNotFoundUpdate")) // Solo añadir si no hubo errores de roles no encontrados
                    {
                        var addRolesResult = await _userManager.AddToRolesAsync(user, existingRolesToAdd);
                        if (!addRolesResult.Succeeded) allErrors.AddRange(addRolesResult.Errors);
                    }
                }
            }

            return allErrors.Any() ? IdentityResult.Failed(allErrors.ToArray()) : IdentityResult.Success;
        }
    }
}