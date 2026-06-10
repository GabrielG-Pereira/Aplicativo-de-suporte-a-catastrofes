namespace Soteria.Sql.Models
{
    public class UsuarioSessao
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty; // Gestor ou Voluntario
    }
}