using BibliotecaApi.DTOs;
using BibliotecaApi.Models;

namespace BibliotecaApi.Interfaces;

public interface iLibroService
{
    
    public List<Libro> getAll();
    public Libro? ObtenerPorId(int id);

    public Libro Crear(LibroCreateDTO nuevoLibro);

    public bool Actualizar(int id, LibroUpdateDTO libroActualizado);

    public bool Eliminar(int id);
    
}