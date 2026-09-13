using Microsoft.AspNetCore.Mvc;
using BibliotecaApi.Models;
using BibliotecaApi.Services;
using BibliotecaApi.DTOs;

namespace BibliotecaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutoresController : ControllerBase
{
    private readonly IAutorService _autorService;

    public AutoresController(IAutorService autorService)
    {
        _autorService = autorService;
    }

    // GET: api/autores
    [HttpGet]
    public ActionResult<IEnumerable<Autor>> GetAll()
    {
        return Ok(_autorService.GetAll());
    }

    // GET: api/autores/{id}
    [HttpGet("{id}")]
    public ActionResult<Autor> GetById(int id)
    {
        Autor autor = _autorService.ObtenerPorId(id);

        if (autor != null)
        {
            return Ok(autor);
        }
        else { return NotFound("Autor no encontrado"); }
    }

    // POST: api/autores
    [HttpPost]
    public ActionResult<Autor> Create([FromBody] AutorDTO nuevoAutor)
    {
        if (string.IsNullOrWhiteSpace(nuevoAutor.Nombre) || string.IsNullOrWhiteSpace(nuevoAutor.Nacionalidad))
        {
            return BadRequest(new { mensaje = "El nombre y la nacionalidad son obligatorios." });
        }

        var autorCreado = _autorService.Crear(nuevoAutor);
        return CreatedAtAction(nameof(GetById), new { id = autorCreado.Id }, autorCreado);
    }

    // PUT: api/autores/{id}
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] AutorDTO autorActualizado)
    {
        var actualizado = _autorService.Actualizar(id, autorActualizado);
        if (actualizado == null)
        {
            return NotFound(new { mensaje = $"No se pudo actualizar. Autor con ID {id} no encontrado." });
        }

        return NoContent(); // 204 No Content para actualizaciones exitosas
    }

    // DELETE: api/autores/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var eliminado = _autorService.Eliminar(id);
        if (!eliminado)
        {
            return NotFound(new { mensaje = $"No se pudo eliminar. Autor con ID {id} no encontrado." });
        }

        return NoContent();
    }
}
