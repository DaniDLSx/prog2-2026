using BibliotecaApi.Models;
using BibliotecaApi.Interfaces;
using BibliotecaApi.DTOs;

namespace BibliotecaApi.Services;

public class LibroFileService: iLibroService
{
    private readonly IFileStorageService _fileStorageService;
    private readonly string _filePath = "Data/libros.json";
    private readonly IAutorService _autorService;

    public LibroFileService(IFileStorageService fileStorageService, IAutorService autorService)
    {
        _fileStorageService = fileStorageService;
        _autorService = autorService;
    }
   
    public bool Actualizar(int id, LibroUpdateDTO libroActualizado)
    {
        var libroExistente = ObtenerPorId(id);
        if (libroExistente == null)
        {
            return false;
        }else
        {
            libroExistente.Titulo = libroActualizado.Titulo;
            libroExistente.ISBN = libroActualizado.ISBN;
            libroExistente.Tema = libroActualizado.Tema;

            var libros = getAll();
            var index = libros.FindIndex(l => l.Id == id);
            if (index != -1)
            {
                libros[index] = libroExistente;
                var librosJson = System.Text.Json.JsonSerializer.Serialize(libros);
                _fileStorageService.Write(_filePath, librosJson);
                return true;
            }
        }
        return false;
    }

    public Libro Crear(LibroCreateDTO nuevoLibro)
    {
        var libros = getAll();
        int nuevoId = libros.Any() ? libros.Max(l => l.Id) + 1 : 1;
        
        Libro libroCreado = new Libro
        {
            Id = nuevoId,
            Titulo = nuevoLibro.Titulo,
            ISBN = nuevoLibro.ISBN,
            Tema = nuevoLibro.Tema,
            Autor = _autorService.ObtenerPorId(nuevoLibro.AutorId),
            IdAutor = nuevoLibro.AutorId
        };
        
        libros.Add(libroCreado);
        var librosJson = System.Text.Json.JsonSerializer.Serialize(libros);
        _fileStorageService.Write(_filePath, librosJson);
        return libroCreado;
    }

    public bool Eliminar(int id)
    {
        var libro = ObtenerPorId(id);
        if (libro == null)
        {
            return false;
        }

        var libros = getAll();
        var index = libros.FindIndex(l => l.Id == id);
        libros.RemoveAt(index);
        var librosJson = System.Text.Json.JsonSerializer.Serialize(libros);
        _fileStorageService.Write(_filePath, librosJson);
        return true;
    }
    

    public List<Libro> getAll()
    {
        var librosJson = _fileStorageService.Read(_filePath);
        return System.Text.Json.JsonSerializer.Deserialize<List<Libro>>(librosJson) ?? new List<Libro>();
    }

    public Libro? ObtenerPorId(int id)
    {
        var libros = getAll();
        return libros.FirstOrDefault(l => l.Id == id);
    }
}