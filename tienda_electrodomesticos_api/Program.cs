using Microsoft.AspNetCore.Authentication.Cookies;
using tienda_electrodomesticos_api.Repositorio.DAO;
using tienda_electrodomesticos_api.Repositorio.Interfaces;
using tienda_electrodomesticos_api.Service.Impl;
using tienda_electrodomesticos_api.Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ----------------------
// Configuración de la cadena de conexión
// ----------------------
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ----------------------
// Registro de DAOs
// ----------------------
builder.Services.AddScoped<ICarritoRepository>(sp => new CarritoDAO(connectionString));
builder.Services.AddScoped<ICategoriaRepository>(sp => new CategoriaDAO(connectionString));
builder.Services.AddScoped<IProductoRepository>(sp => new ProductoDAO(connectionString));
builder.Services.AddScoped<IUsuarioRepository>(sp => new UsuarioDAO(connectionString)); // si Usuario usa ADO.NET

// ----------------------
// Registro de Servicios
// ----------------------
builder.Services.AddScoped<ICarritoService, CarritoService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// ----------------------
// Configuración de autenticación por cookies
// ----------------------
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/signin";
        options.Cookie.Name = "TiendaElectroCookie";
    });

// ----------------------
// Controladores y Swagger
// ----------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ----------------------
// Middlewares
// ----------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
