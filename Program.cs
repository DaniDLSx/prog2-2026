using BibliotecaApi.Services;
using BibliotecaApi.Interfaces;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//Agrego los controladores
builder.Services.AddControllers();

//builder.Services.AddTransient<iLibroService, LibroService>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<iLibroService, LibroFileService>();
builder.Services.AddScoped<IAutorService, AutorFileService>();

// OpenAPI/Swagger para documentación y pruebas de la API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


//Mapeo las rutas de mis controladores
app.MapControllers();
app.Run();


