using Microsoft.AspNetCore.Mvc;
using BibliotecaApi.Models;
using BibliotecaApi.Interfaces;
using BibliotecaApi.DTOs;
using BibliotecaApi.Filters;
using Microsoft.AspNetCore.Authorization;

namespace BibliotecaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibrosController : ControllerBase
{
    private readonly iLibroService _libroService;

    public LibrosController(iLibroService libroService)
    {
        _libroService = libroService;
    }

    // GET: api/libros  (filtros opcionales por autor y tema)
    [HttpGet]
    // Autenticacion de JWT
    [Authorize]
    public async Task<ActionResult<IEnumerable<Libro>>> GetAll([FromQuery] string idautor = "", [FromQuery] string tema = "")
    {
        var libros = await _libroService.getAll();

        if (idautor != "")
        {
            if (tema != "")
                return libros.FindAll(l => l.AutorId == int.Parse(idautor) && l.Tema.Contains(tema, StringComparison.OrdinalIgnoreCase));
            else
                return libros.FindAll(l => l.AutorId == int.Parse(idautor));
        }
        else
        {
            if (tema != "")
                return libros.FindAll(l => l.Tema.Contains(tema, StringComparison.OrdinalIgnoreCase));
        }

        return Ok(libros);
    }

    // GET: api/libros/buscar?termino=...
    [HttpGet("buscar")]
    public async Task<ActionResult<IEnumerable<Libro>>> Search([FromQuery] string termino)
    {
        var libros = await _libroService.getAll();
        return Ok(libros.FindAll(libro => libro.Titulo.Contains(termino, StringComparison.OrdinalIgnoreCase)));
    }

    // GET: api/libros/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Libro>> GetById(int id)
    {
        Libro? libro = await _libroService.ObtenerPorId(id);

        if (libro != null)
            return Ok(libro);
        else
            return NotFound("Libro no encontrado");
    }

    // POST: api/libros
    // Escritura para integración entre APIs: se exige API KEY (header X-API-Key).
    [HttpPost]
    [ApiKey]
    public async Task<ActionResult<Libro>> Create([FromBody] LibroCreateDTO nuevoLibro)
    {
        if (string.IsNullOrWhiteSpace(nuevoLibro.Titulo) || string.IsNullOrWhiteSpace(nuevoLibro.ISBN))
        {
            return BadRequest(new { mensaje = "El título y el ISBN son obligatorios." });
        }

        var libroCreado = await _libroService.Crear(nuevoLibro);
        return CreatedAtAction(nameof(GetById), new { id = libroCreado.Id }, libroCreado);
    }

    // PUT: api/libros/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] LibroUpdateDTO libroActualizado)
    {
        var actualizado = await _libroService.Actualizar(id, libroActualizado);
        if (!actualizado)
        {
            return NotFound(new { mensaje = $"No se pudo actualizar. Libro con ID {id} no encontrado." });
        }

        return NoContent();
    }

    // DELETE: api/libros/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _libroService.Eliminar(id);
        if (!eliminado)
        {
            return NotFound(new { mensaje = $"No se pudo eliminar. Libro con ID {id} no encontrado." });
        }

        return NoContent();
    }
}
