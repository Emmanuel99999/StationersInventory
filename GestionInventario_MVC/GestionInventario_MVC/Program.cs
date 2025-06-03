using GestionInventario_MVC.Services;
using MudBlazor.Services;
using Microsoft.EntityFrameworkCore;
using GestionInventario_MVC.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using GestionInventario_MVC.Areas.Identity.Data;
using GestionInventario_MVC.Services.Interfaces;
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
    options.LoginPath = "/Users/Login";
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
    options.DefaultScheme = IdentityConstants.ApplicationScheme; // o CookieAuthenticationDefaults.AuthenticationScheme
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        // ...etc
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddControllersWithViews();
// -------------------
// Configuración de antiforgery
builder.Services.AddAntiforgery(options => {
    options.HeaderName = "X-CSRF-TOKEN";
});


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
builder.Services.AddScoped<IRoleService, GestionInventario_MVC.Services.Implementations.RoleService>();

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