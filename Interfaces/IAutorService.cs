namespace BibliotecaApi.Services;
using BibliotecaApi.Models;
using BibliotecaApi.DTOs;

public interface IAutorService
{
    public List<Autor> GetAll();
    public Autor? ObtenerPorId(int id);
    public Autor Crear(AutorDTO nuevoAutor);
    public Autor? Actualizar(int id, AutorDTO autorActualizado);
    public bool Eliminar (int id);

}