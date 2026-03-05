namespace API_Arguments
{
    // Enumerados
    public enum Categoria
    {
        Alimentacao,
        Higiene,
        Saude,
        Logistica,
        RH
    }
    public enum Status
    {
        Ativo,
        Encerrado,
        Alerta,
        Critico
    }
    public enum Especialidade
    {
        Medico,
        Enfermeiro,
        Psicologo,
        Logistica,
        RH
    }

    // Interfaces
    public interface IEvento { public int IdEvento { get; set; } }
    public interface IPontoColeta { public int IdPontoColeta { get; set; } }

    // Estruturas
    public struct Coordenada
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
    public struct Telefone
    {
        public int CodigoPais { get; set; }
        public int DDD { get; set; }
        public int Numero { get; set; }
    }
}
