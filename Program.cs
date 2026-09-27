using BibliotecaApi.Services;
using BibliotecaApi.Interfaces;
using BibliotecaApi.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BibliotecaDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("BibliotecaDb")));
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//Agrego los controladores
builder.Services.AddControllers();

builder.Services.AddScoped<iLibroService, LibroDbService>();
//builder.Services.AddScoped<IFileStorageService, FileStorageService>();
//builder.Services.AddScoped<iLibroService, LibroFileService>();
builder.Services.AddScoped<IAutorService, AutorDbService>();

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


