using System.Net.Http.Json;
using BibliotecaApi.DTOs;
using BibliotecaApi.Interfaces;

namespace BibliotecaApi.Services;

// "HttpClient tipado": no creamos el HttpClient a mano (eso trae problemas de
// sockets). .NET nos inyecta uno ya configurado -su BaseAddress se define en
// Program.cs con AddHttpClient-, y nosotros solo lo usamos.
public class BibliotecaExternaService(HttpClient http, ILogger<BibliotecaExternaService> logger)
    : IBibliotecaExternaService
{
    private readonly HttpClient _http = http;
    private readonly ILogger<BibliotecaExternaService> _logger = logger;

    public async Task<List<LibroExternoDTO>> ObtenerLibrosAsync(string termino = "")
    {
        // URL RELATIVA: se concatena con el BaseAddress configurado en Program.cs.
        // Apuntamos a un endpoint anónimo (buscar) para que el ejemplo funcione
        // sin tener que mandar token. termino="" devuelve todos.
        var url = $"api/libros/buscar?termino={Uri.EscapeDataString(termino)}";

        _logger.LogInformation("Consultando API externa: {Base}{Url}", _http.BaseAddress, url);

        // GetFromJsonAsync hace el GET y deserializa el JSON en un solo paso.
        // Si la respuesta no es 2xx, lanza HttpRequestException (la captura el controller).
        var libros = await _http.GetFromJsonAsync<List<LibroExternoDTO>>(url);

        // Si deserializa a null (cuerpo vacío), devolvemos lista vacía en vez de null.
        return libros ?? new List<LibroExternoDTO>();

        // NOTA: si la API del compañero protege este endpoint con [Authorize],
        // habría que mandar el token en el header, por ejemplo:
        //   _http.DefaultRequestHeaders.Authorization =
        //       new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<LibroExternoDTO?> CrearLibroAsync(LibroCreateDTO nuevo)
    {
        // La X-API-Key ya viaja en el header (se configuró en Program.cs con AddHttpClient),
        // así que acá solo hacemos el POST. PostAsJsonAsync serializa el DTO a JSON.
        _logger.LogInformation("POST a la API externa: {Base}api/libros", _http.BaseAddress);

        var resp = await _http.PostAsJsonAsync("api/libros", nuevo);
        resp.EnsureSuccessStatusCode();   // si no es 2xx, lanza HttpRequestException (lo captura el controller)

        return await resp.Content.ReadFromJsonAsync<LibroExternoDTO>();
    }
}
