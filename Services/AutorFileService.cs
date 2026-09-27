namespace BibliotecaApi.Services;
using BibliotecaApi.Interfaces;
using BibliotecaApi.Models;
using BibliotecaApi.DTOs;

public class AutorFileService(IFileStorageService fileStorageService)
{
    private readonly IFileStorageService _fileStorageService = fileStorageService;
    private readonly string _filePath = "Data/autores.json";

    public List<Autor> GetAll()
    {
        var autoresJson = _fileStorageService.Read(_filePath);
        return System.Text.Json.JsonSerializer.Deserialize<List<Autor>>(autoresJson) ?? new List<Autor>();
    }    
    public Autor? Actualizar(int id, AutorDTO autorActualizado)
    {
        var autorExistente = ObtenerPorId(id);
        if (autorExistente == null)
        {
            return null;
        }else
        {
            autorExistente.Nombre = autorActualizado.Nombre;
            autorExistente.Nacionalidad = autorActualizado.Nacionalidad;

            var autores = GetAll();
            var index = autores.FindIndex(l => l.Id == id);
            if (index != -1)
            {
                autores[index] = autorExistente;
                var autoresJson = System.Text.Json.JsonSerializer.Serialize(autores);
                _fileStorageService.Write(_filePath, autoresJson);
                return autorExistente;
            }
        }
        return null;
    }

    public Autor Crear(AutorDTO nuevoAutor)
    {
        var autores = GetAll();
        int nuevoId = autores.Any() ? autores.Max(l => l.Id) + 1 : 1;
        
        Autor autorCreado = new Autor(nuevoId, nuevoAutor.Nombre, nuevoAutor.Nacionalidad);
        
        autores.Add(autorCreado);
        var autoresJson = System.Text.Json.JsonSerializer.Serialize(autores);
        _fileStorageService.Write(_filePath, autoresJson);
        return autorCreado;
    }

    public bool Eliminar(int id)
    {
        var autor = ObtenerPorId(id);
        if (autor == null)
        {
            return false;
        }

        var autores = GetAll();
        var index = autores.FindIndex(a => a.Id == id);
        autores.RemoveAt(index);
        var autoresJson = System.Text.Json.JsonSerializer.Serialize(autores);
        _fileStorageService.Write(_filePath, autoresJson);
        return true;
    }
    

    public Autor? ObtenerPorId(int id)
    {
        var autores = GetAll();
        return autores.FirstOrDefault(a => a.Id == id);
    }
}