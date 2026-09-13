using BibliotecaApi.Models;
using BibliotecaApi.Interfaces;

namespace BibliotecaApi.Services;

public class LibroFileServicev2 : iLibroService
{
    private readonly IFileStorageService _fileStorageService;

    private readonly string _filePath = "Data/libros.json";


    public LibroFileServicev2(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }

    public bool Actualizar(int id, Libro libroActualizado)
    {
        throw new NotImplementedException();
    }

    public Libro Crear(Libro nuevoLibro)
    {
        throw new NotImplementedException();
    }

    public bool Eliminar(int id)
    {
        throw new NotImplementedException();
    }

    public List<Libro> getAll()
    {
         var librosJson = _fileStorageService.Read(_filePath);
        
        return System.Text.Json.JsonSerializer.Deserialize<List<Libro>>(librosJson) ?? new List<Libro>();

    }

    public Libro? ObtenerPorId(int id)
    {
        throw new NotImplementedException();
    }
}