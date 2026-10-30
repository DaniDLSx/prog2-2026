using Microsoft.AspNetCore.Mvc;
using BibliotecaApi.DTOs;
using BibliotecaApi.Interfaces;

namespace BibliotecaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IntegracionController(
    IBibliotecaExternaService externa,
    ILogger<IntegracionController> logger) : ControllerBase
{
    private readonly IBibliotecaExternaService _externa = externa;
    private readonly ILogger<IntegracionController> _logger = logger;

    // GET: api/integracion/libros-externos?termino=redes
    // Trae libros desde OTRA BibliotecaApi (la de ApiExterna:BaseUrl en appsettings).
    [HttpGet("libros-externos")]
    public async Task<ActionResult<IEnumerable<LibroExternoDTO>>> GetLibrosExternos([FromQuery] string termino = "")
    {
        try
        {
            var libros = await _externa.ObtenerLibrosAsync(termino);
            return Ok(libros);
        }
        catch (HttpRequestException ex)
        {
            // La API externa falló o no respondió. Nosotros estamos bien, por eso
            // devolvemos 502 (Bad Gateway): el servicio del que dependemos falló.
            _logger.LogError(ex, "Fallo al consultar la API externa");
            return StatusCode(StatusCodes.Status502BadGateway,
                new { mensaje = "No se pudo consultar la API externa.", detalle = ex.Message });
        }
    }

    // POST: api/integracion/libros-externos
    // Crea un libro en la OTRA BibliotecaApi. El servicio manda la X-API-Key en el header.
    [HttpPost("libros-externos")]
    public async Task<ActionResult<LibroExternoDTO>> CrearLibroExterno([FromBody] LibroCreateDTO nuevo)
    {
        try
        {
            var creado = await _externa.CrearLibroAsync(nuevo);
            return Ok(creado);
        }
        catch (HttpRequestException ex)
        {
            // Incluye el caso de API key inválida (la externa responde 401) y cualquier otro fallo.
            _logger.LogError(ex, "Fallo al crear en la API externa");
            return StatusCode(StatusCodes.Status502BadGateway,
                new { mensaje = "No se pudo crear el libro en la API externa.", detalle = ex.Message });
        }
    }
}
