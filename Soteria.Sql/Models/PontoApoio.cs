namespace Soteria.Sql.Models
{
    public class PontoApoio
    {
        public Guid Id { get; set; }

        public string Nome { get; set; } = "";

        public string Endereco { get; set; } = "";

        public string Contato { get; set; } = "";

        public string Tipo { get; set; } = "";

        public double DistanciaKm { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
