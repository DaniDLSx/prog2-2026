namespace BibliotecaApi.Models
{
   public class Usuario{
        public int Id { get; set; }
        public string NombreUsuario { get; set; } = "";
        public string PasswordHash { get; set; } = ""; // nunca la contraseña en claro
        public string Rol { get; set; } = "Usuario"; // "Usuario" | "Administrador"
    }
}