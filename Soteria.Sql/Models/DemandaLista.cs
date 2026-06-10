namespace Soteria.Sql.Models
{
    public class DemandaLista
    {
        public Guid Id { get; set; }
        public string CategoriaNome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}