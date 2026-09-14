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
    //Agregamos parametro FromQuery para poder filtrar por idAutor y Tema
    [HttpGet]
    public ActionResult<IEnumerable<Libro>> GetAll([FromQuery] string idautor = "", [FromQuery] string tema = "")
    {
       //return BadRequest(new { mensaje = "Error en la solicitud." });
        if (idautor != "")
        {
                if(tema != "")
                {
                    return _libroService.getAll().FindAll(l => l.IdAutor == int.Parse(idautor) && l.Tema.Contains(tema, StringComparison.OrdinalIgnoreCase));
                }else
                {
                    return _libroService.getAll().FindAll(l => l.IdAutor == int.Parse(idautor) );
                }
        }
        else
        {
            if(tema != "")
                    return _libroService.getAll().FindAll(l => l.Tema.Contains(tema, StringComparison.OrdinalIgnoreCase));    
        }
        
        return Ok(_libroService.getAll());
    } 

    //Parametro FromQuery que filtra los libros por titulo segun el parametro recibido
    [HttpGet("buscar")]
        public ActionResult<IEnumerable<Libro>> Search([FromQuery] string termino)
    {
       //return BadRequest(new { mensaje = "Error en la solicitud." });
        return Ok(_libroService.getAll().FindAll(libro => libro.Titulo.Contains(termino, StringComparison.OrdinalIgnoreCase) ));
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

