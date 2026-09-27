using BibliotecaApi.DTOs;
using BibliotecaApi.Models;

namespace BibliotecaApi.Interfaces;

public interface iLibroService
{
    public Task<List<Libro>> getAll();
    public Task<Libro?> ObtenerPorId(int id);
    public Task<Libro> Crear(LibroCreateDTO nuevoLibro);
    public Task<bool> Actualizar(int id, LibroUpdateDTO libroActualizado);
    public Task<bool> Eliminar(int id);
}
