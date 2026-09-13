namespace BibliotecaApi.DTOs;

public class LibroCreateDTO
{
    public string Titulo { get; set; }
    public int AutorId { get; set; } // Nuevo campo para almacenar el ID del autor
    public string ISBN { get; set; }
    public string Tema { get; set; }
}

//Decidimos como logica de negocio que el usuario no puede cambiar el autor de un libro ya creado,
// por lo que no incluimos el campo AutorId en el DTO de actualización.
public class LibroUpdateDTO
{
    public string Titulo { get; set; }
    public string ISBN { get; set; }
    public string Tema { get; set; }
}