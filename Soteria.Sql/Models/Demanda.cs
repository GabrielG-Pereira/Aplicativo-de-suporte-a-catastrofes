namespace Soteria.Sql.Models
{
    public enum StatusDemanda
    {
        Medio,
        Estavel,
        Critico
    }

    public class Demanda
    {
        public Guid Id { get; set; }

        public Guid IdPontoEvento { get; set; }

        public string Nome { get; set; } = "";

        public string Descricao { get; set; } = "";

        public StatusDemanda Status { get; set; }

        public Guid Categoria { get; set; }
    }
}
