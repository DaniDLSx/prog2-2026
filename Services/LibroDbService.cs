using BibliotecaApi.Models;
using BibliotecaApi.Interfaces;
using BibliotecaApi.DTOs;
using BibliotecaApi.Context;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaApi.Services;

public class LibroDbService(BibliotecaDbContext context) : iLibroService
{
    private readonly BibliotecaDbContext _context = context;

    public async Task<List<Libro>> getAll()
    {
        return await _context.Libros.ToListAsync();
    }

    public async Task<Libro?> ObtenerPorId(int id)
    {
        return await _context.Libros.FindAsync(id);
    }

    public async Task<Libro> Crear(LibroCreateDTO nuevoLibro)
    {
        Libro nuevo = new Libro
        {
            Titulo = nuevoLibro.Titulo,
            AutorId = nuevoLibro.AutorId,
            ISBN = nuevoLibro.ISBN,
            Tema = nuevoLibro.Tema
        };
        _context.Libros.Add(nuevo);
        await _context.SaveChangesAsync();
        return nuevo;
    }

    public async Task<bool> Actualizar(int id, LibroUpdateDTO libroActualizado)
    {
        Libro? libro = await _context.Libros.FindAsync(id);
        if (libro == null) return false;

        // Regla de negocio: no se cambia el autor de un libro ya creado.
        libro.Titulo = libroActualizado.Titulo;
        libro.ISBN = libroActualizado.ISBN;
        libro.Tema = libroActualizado.Tema;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Eliminar(int id)
    {
        Libro? libro = await _context.Libros.FindAsync(id);
        if (libro == null) return false;

        _context.Libros.Remove(libro);
        await _context.SaveChangesAsync();
        return true;
    }
}
