using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GestionInventario_MVC.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace GestionInventario_MVC.IntegrationTests
{
    public class RolesApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        // Cambia aquí el usuario y la contraseña de prueba a los correctos de tu proyecto
        private const string TestUsername = "Admintest@gmail.com";
        private const string TestPassword = "Admin123!";

        public RolesApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        // Método auxiliar para obtener el JWT
        private async Task<string> GetJwtTokenAsync()
        {
            var loginPayload = new
            {
                Username = TestUsername,
                Password = TestPassword
            };
            var content = new StringContent(
                JsonSerializer.Serialize(loginPayload),
                Encoding.UTF8,
                "application/json"
            );
            // Asegúrate de que la ruta sea la correcta de tu API
            var response = await _client.PostAsync("/api/auth/login", content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            // Asegúrate de cambiar "token" si tu respuesta tiene un campo distinto
            var token = doc.RootElement.GetProperty("token").GetString();
            return token;
        }

        [Fact]
        public async Task GetAll_ReturnsSuccessStatusCode()
        {
            // 1. Autenticarse y obtener el token
            var token = await GetJwtTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // 2. Realizar la petición autenticada
            var response = await _client.GetAsync("/api/rolesapi");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateAndGetById_ReturnsRole()
        {
            // 1. Autenticarse y obtener el token
            var token = await GetJwtTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Arrange
            var roleToCreate = new RoleDto { Name = "IntegrationTestRole" };
            var httpContent = new StringContent(
                JsonSerializer.Serialize(roleToCreate),
                Encoding.UTF8,
                "application/json"
            );

            // Act - Crear
            var createResponse = await _client.PostAsync("/api/rolesapi", httpContent);
            createResponse.EnsureSuccessStatusCode();

            // Obtener todos los roles
            var getAllResponse = await _client.GetAsync("/api/rolesapi");
            getAllResponse.EnsureSuccessStatusCode();
            var stream = await getAllResponse.Content.ReadAsStreamAsync();
            var roles = await JsonSerializer.DeserializeAsync<List<RoleDto>>(stream,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var createdRole = roles.FirstOrDefault(r => r.Name == "IntegrationTestRole");
            Assert.NotNull(createdRole);

            // Act - Obtener por ID
            var response = await _client.GetAsync($"/api/rolesapi/{createdRole.Id}");
            response.EnsureSuccessStatusCode();
            var role = await JsonSerializer.DeserializeAsync<RoleDto>(
                await response.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            Assert.Equal("IntegrationTestRole", role.Name);

            // Limpieza (opcional)
            await _client.DeleteAsync($"/api/rolesapi/{createdRole.Id}");
        }
    }
}