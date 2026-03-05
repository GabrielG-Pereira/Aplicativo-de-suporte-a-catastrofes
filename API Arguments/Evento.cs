namespace API_Arguments
{
    public class Evento
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public Coordenada Local { get; set; }
        public Status Status { get; set; }
        public DateTime DataInicio { get; set; }
    }
}
