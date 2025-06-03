using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestionInventario_MVC.Controllers.Api;
using GestionInventario_MVC.DTOs;
using GestionInventario_MVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GestionInventario_MVC.Test.Controllers.Api
{
    public class RolesApiControllerTests
    {
        private readonly Mock<IRoleService> _mockRoleService;
        private readonly RolesApiController _controller;

        public RolesApiControllerTests()
        {
            _mockRoleService = new Mock<IRoleService>();
            _controller = new RolesApiController(_mockRoleService.Object);
        }

        // GET ALL TESTS
        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfRoles()
        {
            // Arrange
            var rolesList = new List<RoleDto>
            {
                new RoleDto { Id = "1", Name = "Admin" },
                new RoleDto { Id = "2", Name = "User" }
            };

            _mockRoleService.Setup(x => x.GetAllAsync()).ReturnsAsync(rolesList);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<RoleDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetAll_ReturnsEmptyList_WhenNoRolesExist()
        {
            // Arrange
            _mockRoleService.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<RoleDto>());

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<RoleDto>>(okResult.Value);
            Assert.Empty(returnValue);
        }

        // GET BY ID TESTS
        [Fact]
        public async Task GetById_ReturnsOkResult_WithRole_WhenRoleExists()
        {
            // Arrange
            var roleDto = new RoleDto { Id = "1", Name = "Admin" };
            _mockRoleService.Setup(x => x.GetByIdAsync("1")).ReturnsAsync(roleDto);

            // Act
            var result = await _controller.GetById("1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<RoleDto>(okResult.Value);
            Assert.Equal("Admin", returnValue.Name);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenRoleDoesNotExist()
        {
            // Arrange
            _mockRoleService.Setup(x => x.GetByIdAsync("999")).ReturnsAsync((RoleDto)null);

            // Act
            var result = await _controller.GetById("999");

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        // CREATE TESTS
        [Fact]
        public async Task Create_ReturnsCreatedResult_WhenRoleIsCreated()
        {
            // Arrange
            var roleDto = new RoleDto { Name = "NewRole" };
            _mockRoleService.Setup(x => x.CreateAsync("NewRole")).ReturnsAsync(true);

            // Act
            var result = await _controller.Create(roleDto);

            // Assert
            var createdResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenRoleNameIsNull()
        {
            // Arrange
            RoleDto roleDto = new RoleDto { Name = null };

            // Act
            var result = await _controller.Create(roleDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        // UPDATE TESTS
        [Fact]
        public async Task Update_ReturnsOkResult_WhenRoleIsUpdated()
        {
            // Arrange
            var roleDto = new RoleDto { Id = "1", Name = "UpdatedRole" };
            _mockRoleService.Setup(x => x.UpdateAsync("1", "UpdatedRole")).ReturnsAsync(true);

            // Act
            var result = await _controller.Update("1", roleDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenRoleUpdateFails()
        {
            // Arrange
            var roleDto = new RoleDto { Id = "1", Name = "UpdatedRole" };
            _mockRoleService.Setup(x => x.UpdateAsync("1", "UpdatedRole")).ReturnsAsync(false);

            // Act
            var result = await _controller.Update("1", roleDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        // DELETE TESTS
        [Fact]
        public async Task Delete_ReturnsOkResult_WhenRoleIsDeleted()
        {
            // Arrange
            _mockRoleService.Setup(x => x.DeleteAsync("1")).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete("1");

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenRoleDeletionFails()
        {
            // Arrange
            _mockRoleService.Setup(x => x.DeleteAsync("999")).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete("999");

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
