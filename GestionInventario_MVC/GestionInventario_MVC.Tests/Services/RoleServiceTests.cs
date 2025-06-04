using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionInventario_MVC.DTOs;
using GestionInventario_MVC.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace GestionInventario_MVC.Test.services
{
	public class RoleServiceTests
	{
		private readonly Mock<RoleManager<IdentityRole>> _mockRoleManager;

		public RoleServiceTests()
		{
			var store = new Mock<IRoleStore<IdentityRole>>();
			_mockRoleManager = new Mock<RoleManager<IdentityRole>>(
				store.Object, null, null, null, null);
		}

		[Fact]
		public async Task CreateAsync_Should_Return_True_When_Successful()
		{
			_mockRoleManager.Setup(x => x.RoleExistsAsync("Admin")).ReturnsAsync(false);
			_mockRoleManager.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>()))
							.ReturnsAsync(IdentityResult.Success);

			var service = new RoleService(_mockRoleManager.Object);
			var result = await service.CreateAsync("Admin");

			Assert.True(result);
		}

		[Fact]
		public async Task CreateAsync_Should_Return_False_If_Exists()
		{
			_mockRoleManager.Setup(x => x.RoleExistsAsync("Admin")).ReturnsAsync(true);

			var service = new RoleService(_mockRoleManager.Object);
			var result = await service.CreateAsync("Admin");

			Assert.False(result);
		}

		[Fact]
		public async Task DeleteAsync_Should_Return_False_When_Not_Found()
		{
			_mockRoleManager.Setup(x => x.FindByIdAsync("123")).ReturnsAsync((IdentityRole)null);

			var service = new RoleService(_mockRoleManager.Object);
			var result = await service.DeleteAsync("123");

			Assert.False(result);
		}

		[Fact]
		public async Task GetAllAsync_Should_Return_AllRoles()
		{
			var roles = new List<IdentityRole>
			{
				new IdentityRole { Id = "1", Name = "Admin" },
				new IdentityRole { Id = "2", Name = "User" }
			}.AsQueryable();

			_mockRoleManager.Setup(x => x.Roles).Returns(roles);

			var service = new RoleService(_mockRoleManager.Object);
			var result = await service.GetAllAsync();

			Assert.Equal(2, result.Count);
			Assert.Contains(result, r => r.Name == "Admin");
		}

		[Fact]
		public async Task GetByIdAsync_Should_Return_Role_WhenExists()
		{
			var role = new IdentityRole { Id = "1", Name = "Admin" };

			_mockRoleManager.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(role);

			var service = new RoleService(_mockRoleManager.Object);
			var result = await service.GetByIdAsync("1");

			Assert.NotNull(result);
			Assert.Equal("Admin", result?.Name);
		}

		[Fact]
		public async Task UpdateAsync_Should_Return_True_When_Successful()
		{
			var role = new IdentityRole { Id = "1", Name = "OldName" };

			_mockRoleManager.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(role);
			_mockRoleManager.Setup(x => x.RoleExistsAsync("NewName")).ReturnsAsync(false);
			_mockRoleManager.Setup(x => x.UpdateAsync(It.IsAny<IdentityRole>()))
							.ReturnsAsync(IdentityResult.Success);

			var service = new RoleService(_mockRoleManager.Object);
			var result = await service.UpdateAsync("1", "NewName");

			Assert.True(result);
		}
	}
}
