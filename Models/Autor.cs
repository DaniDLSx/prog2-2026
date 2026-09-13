namespace BibliotecaApi.Models;

public class Autor
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Nacionalidad { get; set; }

    public Autor(int id, string nombre, string nacionalidad)
    {
        Id = id;
        Nombre = nombre;
        Nacionalidad = nacionalidad;
    }
}


