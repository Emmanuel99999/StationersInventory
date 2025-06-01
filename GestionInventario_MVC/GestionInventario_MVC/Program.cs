using GestionInventario_MVC.Services;
using MudBlazor.Services;
using Microsoft.EntityFrameworkCore;
using GestionInventario_MVC.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using GestionInventario_MVC.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen; // En algunos casos podría ser necesario

// OJO: Asegúrate que AppDbContext herede de IdentityDbContext<GestionInventario_MVCUser>

var builder = WebApplication.CreateBuilder(args);

// -------------------
// 1. Agregar servicios básicos (Razor, Blazor, MudBlazor, MVC)
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddControllersWithViews();
builder.Services.AddControllers(); // <--- necesario para APIs

// -------------------
// 2. Configurar Entity Framework e Identity
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<GestionInventario_MVCUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddTransient<IEmailSender, EmailSender>();

// -------------------
// 3. Configuración personalizada de cookies (opcional pero recomendado)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

// -------------------
// 4. Autenticación JWT para las APIs
// Poner un token/jwt seguro en tu appsettings.json y reemplazarlo aquí, o usa UserSecrets
var jwtKey = builder.Configuration["Jwt:Key"] ?? "CAMBIA_ESTA_LLAVE_POR_ALGO_BIEN_LARGO_Y_SEGURO";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "GestionInventarioAPI";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "GestionInventarioCliente";

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddCookie(options => { // <- Para la web
    options.LoginPath = "/Users/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
})
.AddJwtBearer(options => { // <- Para la API
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
    };
});

// -------------------
// 5. CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins("https://tudominio.com", "https://localhost:PORT") // Cambia a tus orígenes front-end/API si hace falta
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// -------------------
// 6. Swagger + JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GestionInventario API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Introduce el token JWT así: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// -------------------
// 7. Servicios personalizados
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICompraService, CompraService>();

// -------------------
// 8. Construir la aplicación
var app = builder.Build();

// -------------------
// 9. Configuración del pipeline de la aplicación
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowSpecificOrigins");

app.UseAuthentication();
app.UseAuthorization();

// Swagger solo en dev - o protégelo con [Authorize] si deseas
app.UseSwagger();
app.UseSwaggerUI();

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/" &&
        !context.Request.Query.ContainsKey("fromHome"))
    {
        context.Response.Redirect("/home/index");
        return;
    }
    await next();
});

// -------------------
// 10. Mapear endpoints
app.MapBlazorHub();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllers(); // <--- Asegúrate de incluir esto para exponer tus APIs
app.MapRazorPages();
app.MapFallbackToPage("/_Host");

app.Run();