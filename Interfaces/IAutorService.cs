namespace BibliotecaApi.Services;
using BibliotecaApi.Models;
using BibliotecaApi.DTOs;

public interface IAutorService
{
    public Task<List<Autor>> GetAll();
    public Task<Autor?> ObtenerPorId(int id);
    public Task<Autor> Crear(AutorDTO nuevoAutor);
    public Task<Autor?> Actualizar(int id, AutorDTO autorActualizado);
    public Task<bool> Eliminar (int id);

}