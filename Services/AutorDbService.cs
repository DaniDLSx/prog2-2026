using BibliotecaApi.Models;
using BibliotecaApi.Interfaces;
using BibliotecaApi.DTOs;
using BibliotecaApi.Context;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaApi.Services;


public class AutorDbService(BibliotecaDbContext context) : IAutorService
{
    private readonly BibliotecaDbContext _context = context;

    public async Task<List<Autor>> GetAll()
    {
        return await _context.Autores.ToListAsync();
    }
    public async Task<Autor?> ObtenerPorId(int id)
    {
        return await _context.Autores.FindAsync(id);
    }
    public async Task<Autor> Crear(AutorDTO autorDto)
    {
        Autor nuevoAutor = new Autor
        {
            Nombre = autorDto.Nombre,
            Nacionalidad = autorDto.Nacionalidad
        };
        _context.Autores.Add(nuevoAutor);
        await _context.SaveChangesAsync();
        return nuevoAutor;
    }
    public async Task<Autor?> Actualizar(int id, AutorDTO autorActualizado)
    {
        Autor ? autorExistente = await ObtenerPorId(id);
        if (autorExistente == null) return null;

        autorExistente.Nombre = autorActualizado.Nombre;
        autorExistente.Nacionalidad = autorActualizado.Nacionalidad;

        await _context.SaveChangesAsync();
        return autorExistente;
    }
    public async Task<bool> Eliminar (int id)
    {
        Autor ? autorExistente = await ObtenerPorId(id);
        if (autorExistente == null) return false;

        _context.Autores.Remove(autorExistente);
        await _context.SaveChangesAsync();
        return true;
    }

}