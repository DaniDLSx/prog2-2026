namespace BibliotecaApi.Controllers;
using BibliotecaApi.Context;
using BibliotecaApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BibliotecaApi.DTOs;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

[ApiController]
[Route("api/[controller]")]
public class AuthController(BibliotecaDbContext context, IConfiguration config) : ControllerBase
{
    private readonly BibliotecaDbContext _context = context;
    private readonly IConfiguration _config = config;
    private readonly PasswordHasher<Usuario> _hasher = new();

    [HttpPost("register")]
    public async Task <IActionResult> Register(RegistroDto registroDto)
    {
        //Validar el usuario ya existe
        if (await _context.Usuarios.AnyAsync(u => u.NombreUsuario == registroDto.Usuario))
        return Conflict("Ese usuario ya existe.");

        Usuario nuevoUsuario = new Usuario{
            NombreUsuario = registroDto.Usuario,
            Rol = "Usuario"
        };
        nuevoUsuario.PasswordHash = _hasher.HashPassword(null, registroDto.Password);

        _context.Usuarios.Add(nuevoUsuario);
        await _context.SaveChangesAsync();

        return Ok(new { Id = nuevoUsuario.Id, Usuario = nuevoUsuario.NombreUsuario });
        
    }

    [HttpPost("login")]
    public async Task<ActionResult<object>> Login(LoginDto loginDto)
    {
        Usuario? usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == loginDto.Usuario);
        if (usuario == null) return Unauthorized("Usuario o contraseña incorrectos");

        var resultado = _hasher.VerifyHashedPassword(usuario, usuario.PasswordHash, loginDto.Password);
        if (resultado == PasswordVerificationResult.Failed) return Unauthorized("Credenciales inválidas.");

        return Ok(new { Token = GenerarToken(usuario) });
    }

    private string GenerarToken(Usuario usuario)
    {
        var jwt = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.NombreUsuario),
            new Claim(ClaimTypes.Role, usuario.Rol), // el rol sale de la base
            new Claim("Entdida", "UAP") 
        };
        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(jwt["ExpiraMinutos"]!)),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

