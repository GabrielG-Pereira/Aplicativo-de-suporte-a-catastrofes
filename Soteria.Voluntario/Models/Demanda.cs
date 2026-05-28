namespace Soteria.Voluntario.Models
{
    public class Demanda
    {
        public int       IdPontoColeta { get; set; }
        public int       IdEvento      { get; set; }
        public Categoria Categoria     { get; set; }
        public string    Descricao     { get; set; } = string.Empty;
        public int       Quantidade    { get; set; }
    }
}
