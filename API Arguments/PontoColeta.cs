using System.Text.Json.Serialization;

namespace API_Arguments
{
    public class PontoColeta
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public Coordenada Local { get; set; }
        public List<Telefone> Telefone { get; set; } = [];

        [JsonIgnore]
        public List<Demanda> Demandas { get; set; } = [];
    }
}
