namespace Soteria.Voluntario.Models
{
    public class Voluntario
    {
        public int          Id            { get; set; }
        public string       Nome          { get; set; } = string.Empty;
        public string       Email         { get; set; } = string.Empty;
        public string       Contato       { get; set; } = string.Empty;
        public string       Endereco      { get; set; } = string.Empty;
        public Especialidade Especialidade { get; set; }
        public bool         Disponivel    { get; set; }
        public Coordenada   Local         { get; set; }
    }
}
