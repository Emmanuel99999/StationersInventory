using GestionInventario_MVC.Services;
using MudBlazor.Services;
using Microsoft.EntityFrameworkCore;
using GestionInventario_MVC.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using GestionInventario_MVC.Areas.Identity.Data;
// OJO: Asegúrate que AppDbContext herede de IdentityDbContext<GestionInventario_MVCUser>

var builder = WebApplication.CreateBuilder(args);

// -------------------
// 1. Agregar servicios básicos (Razor, Blazor, MudBlazor, MVC)
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddControllersWithViews();

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
// 4. Servicios personalizados
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICompraService, CompraService>();

// -------------------
// 5. Construir la aplicación
var app = builder.Build();

// -------------------
// 6. Configuración del pipeline de la aplicación
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting(); // Routing debe ir antes que authentication y authorization

app.UseAuthentication();
app.UseAuthorization();

// Middleware alternativo (para redirigir "/" a /home/index con cierto control)
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
// 7. Mapear endpoints
app.MapBlazorHub();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapFallbackToPage("/_Host");

app.Run();