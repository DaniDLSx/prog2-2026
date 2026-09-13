using Microsoft.AspNetCore.Mvc;
using BibliotecaApi.Models;
using BibliotecaApi.Interfaces;
using BibliotecaApi.DTOs;

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

    

    //Get para obtener todos los libros
    [HttpGet]
    public ActionResult<IEnumerable<Libro>> GetAll()
    {
       //return BadRequest(new { mensaje = "Error en la solicitud." });
        return Ok(_libroService.getAll());
    } 

    // Get con dato enviado en la ruta para filtrar un solo libro
    [HttpGet("{id}")]
    public  ActionResult<Libro> GetById(int id)
    {
        Libro libro = _libroService.ObtenerPorId(id);

        if(libro != null)
        {
            return Ok(libro);
        }
        else{ return NotFound("Libro no encontrado");}
    }

    // POST: api/libros
    [HttpPost]
    public ActionResult<Libro> Create([FromBody] LibroCreateDTO nuevoLibro)
    {
        if (string.IsNullOrWhiteSpace(nuevoLibro.Titulo) || string.IsNullOrWhiteSpace(nuevoLibro.ISBN))
        {
            return BadRequest(new { mensaje = "El título y el ISBN son obligatorios." });
        }

        var libroCreado = _libroService.Crear(nuevoLibro);
        return CreatedAtAction(nameof(GetById), new { id = libroCreado.Id }, libroCreado);
    }


    // PUT: api/libros/{id}
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] LibroUpdateDTO libroActualizado)
    {
        var actualizado = _libroService.Actualizar(id, libroActualizado);
        if (!actualizado)
        {
            return NotFound(new { mensaje = $"No se pudo actualizar. Libro con ID {id} no encontrado." });
        }

        return NoContent(); // 204 No Content para actualizaciones exitosas
    }

    // DELETE: api/libros/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var eliminado = _libroService.Eliminar(id);
        if (!eliminado)
        {
            return NotFound(new { mensaje = $"No se pudo eliminar. Libro con ID {id} no encontrado." });
        }

        return NoContent();
    }
}

