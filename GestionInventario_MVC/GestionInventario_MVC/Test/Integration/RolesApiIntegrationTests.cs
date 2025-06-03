using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GestionInventario_MVC.DTOs;
using Xunit;

namespace GestionInventario_MVC.Test.Integration
{
    public class RolesApiIntegrationTests
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "api/rolesapi";
        private readonly string _baseAddress = "https://localhost:7001"; // Ajusta al puerto de tu aplicación

        public RolesApiIntegrationTests()
        {
            // Configurar HttpClient para las pruebas directas
            _client = new HttpClient
            {
                BaseAddress = new Uri(_baseAddress)
            };

            // Simular un token de autenticación (podrías generar uno real si lo necesitas)
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "test-token-for-integration-tests");

            // Agregar cabeceras adicionales si son necesarias
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [Fact(Skip = "Requiere una API en ejecución")]
        public async Task GetAll_ReturnsSuccessStatusCode()
        {
            var response = await _client.GetAsync(_baseUrl);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(Skip = "Requiere una API en ejecución")]
        public async Task GetById_WithValidId_ReturnsRole()
        {
            // Arrange: crear un rol primero
            var roleToCreate = new RoleDto { Name = "TestRole" };
            var createContent = new StringContent(
                JsonSerializer.Serialize(roleToCreate),
                Encoding.UTF8,
                "application/json");

            var createResponse = await _client.PostAsync(_baseUrl, createContent);
            createResponse.EnsureSuccessStatusCode();

            // Obtener la lista para extraer el id del rol creado
            var getAllResponse = await _client.GetAsync(_baseUrl);
            var roles = await JsonSerializer.DeserializeAsync<List<RoleDto>>(
                await getAllResponse.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var createdRole = roles.FirstOrDefault(r => r.Name == "TestRole");
            Assert.NotNull(createdRole);

            // Act: obtener el rol por id
            var response = await _client.GetAsync($"{_baseUrl}/{createdRole.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var role = await JsonSerializer.DeserializeAsync<RoleDto>(
                await response.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal("TestRole", role.Name);

            // Cleanup
            await _client.DeleteAsync($"{_baseUrl}/{createdRole.Id}");
        }

        [Fact(Skip = "Requiere una API en ejecución")]
        public async Task Create_WithValidRole_ReturnsSuccessStatusCode()
        {
            // Arrange
            var newRole = new RoleDto { Name = "NewIntegrationTestRole" };
            var content = new StringContent(
                JsonSerializer.Serialize(newRole),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync(_baseUrl, content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Cleanup
            var getAllResponse = await _client.GetAsync(_baseUrl);
            var roles = await JsonSerializer.DeserializeAsync<List<RoleDto>>(
                await getAllResponse.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var createdRole = roles.FirstOrDefault(r => r.Name == "NewIntegrationTestRole");
            if (createdRole != null)
            {
                await _client.DeleteAsync($"{_baseUrl}/{createdRole.Id}");
            }
        }

        [Fact(Skip = "Requiere una API en ejecución")]
        public async Task Update_WithValidRole_ReturnsOkStatusCode()
        {
            // Arrange: crear rol
            var roleToCreate = new RoleDto { Name = "RoleToUpdate" };
            var createContent = new StringContent(
                JsonSerializer.Serialize(roleToCreate),
                Encoding.UTF8,
                "application/json");

            var createResponse = await _client.PostAsync(_baseUrl, createContent);
            createResponse.EnsureSuccessStatusCode();

            // Obtener id del rol creado
            var getAllResponse = await _client.GetAsync(_baseUrl);
            var roles = await JsonSerializer.DeserializeAsync<List<RoleDto>>(
                await getAllResponse.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var createdRole = roles.FirstOrDefault(r => r.Name == "RoleToUpdate");
            Assert.NotNull(createdRole);

            // Preparar actualización
            var updateRole = new RoleDto { Id = createdRole.Id, Name = "UpdatedIntegrationTestRole" };
            var updateContent = new StringContent(
                JsonSerializer.Serialize(updateRole),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PutAsync($"{_baseUrl}/{createdRole.Id}", updateContent);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Cleanup
            await _client.DeleteAsync($"{_baseUrl}/{createdRole.Id}");
        }

        [Fact(Skip = "Requiere una API en ejecución")]
        public async Task Delete_WithValidId_ReturnsOkStatusCode()
        {
            // Arrange: crear rol
            var roleToCreate = new RoleDto { Name = "RoleToDelete" };
            var createContent = new StringContent(
                JsonSerializer.Serialize(roleToCreate),
                Encoding.UTF8,
                "application/json");

            var createResponse = await _client.PostAsync(_baseUrl, createContent);
            createResponse.EnsureSuccessStatusCode();

            // Obtener id del rol creado
            var getAllResponse = await _client.GetAsync(_baseUrl);
            var roles = await JsonSerializer.DeserializeAsync<List<RoleDto>>(
                await getAllResponse.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var createdRole = roles.FirstOrDefault(r => r.Name == "RoleToDelete");
            Assert.NotNull(createdRole);

            // Act
            var response = await _client.DeleteAsync($"{_baseUrl}/{createdRole.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
