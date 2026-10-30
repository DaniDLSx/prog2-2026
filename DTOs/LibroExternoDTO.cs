namespace BibliotecaApi.DTOs;

// DTO de ENTRADA: modela la respuesta que esperamos de la API externa
// (por ejemplo, la BibliotecaApi de un compañero). Solo incluimos los
// campos que nos interesa leer; si la otra API manda más, se ignoran.
public class LibroExternoDTO
{
    public int Id { get; set; }
    public string Titulo { get; set; } = "";
    public string ISBN { get; set; } = "";
    public string Tema { get; set; } = "";
    public int AutorId { get; set; }
}
