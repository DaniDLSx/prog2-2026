using BibliotecaApi.DTOs;

namespace BibliotecaApi.Interfaces;

// Contrato para consumir OTRA BibliotecaApi por HTTP (la de un compañero).
// Separar esto en una interfaz nos permite inyectarlo y, si hiciera falta,
// reemplazar la implementación o simularla en tests.
public interface IBibliotecaExternaService
{
    // Pide la lista de libros a la API externa. 'termino' filtra por título.
    Task<List<LibroExternoDTO>> ObtenerLibrosAsync(string termino = "");

    // Crea (POST) un libro en la API externa. La X-API-Key viaja en el header,
    // configurada en el HttpClient (ver Program.cs).
    Task<LibroExternoDTO?> CrearLibroAsync(LibroCreateDTO nuevo);
}
