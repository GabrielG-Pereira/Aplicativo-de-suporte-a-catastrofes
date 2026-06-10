namespace Soteria.Sql.Models
{
    public class PontoColetaLista
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool Temporario { get; set; }
    }
}