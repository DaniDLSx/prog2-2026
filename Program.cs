using BibliotecaApi.Services;
using BibliotecaApi.Interfaces;
using BibliotecaApi.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

//Autenticacion de JWT
var jwt = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwt["Key"]!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwt["Audience"],
            ValidateLifetime = true, // rechaza tokens vencidos
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    }
);

builder.Services.AddAuthorization();


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
builder.Services.AddSwaggerGen(options =>
{
    // 1) Declara el esquema: un Bearer Token (JWT) que viaja en el header
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Pegá SOLO el token JWT (sin escribir 'Bearer ' adelante)."
    });
        // 2) Aplica ese esquema como requisito para los endpoints protegidos
        options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
        {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication(); // ¿quién sos? lee y valida el token
app.UseAuthorization(); // ¿qué podés? ap
app.UseHttpsRedirection();


//Mapeo las rutas de mis controladores
app.MapControllers();
app.Run();


