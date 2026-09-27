namespace BibliotecaApi.Models
{
    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public Autor? Autor { get; set; }
        public int AutorId { get; set; } // FK: ID del autor
        public string ISBN {get; set;}

        public string Tema {get; set;}

        //Nuevo
        //public List<Capitulo> Capitulos { get; set; } = new();
        //
        public Libro(int id, string titulo, int autorId, string isbn, string tema)
        {
            Id = id;
            Titulo = titulo;
            AutorId = autorId;
            ISBN = isbn;
            Tema = tema;            
        }

        public Libro()
        {
            
        }

    }
}