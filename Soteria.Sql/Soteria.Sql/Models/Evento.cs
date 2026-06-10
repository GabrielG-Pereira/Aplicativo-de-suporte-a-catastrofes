using System;
using System.Collections.Generic;
using System.Text;

namespace Soteria.Sql.Models
{
    public class Evento
    {
        public Guid Id { get; set; }

        public string Nome { get; set; } = "";

        public string Descricao { get; set; } = "";

        public string Status { get; set; } = "";

        public DateTime DataInicio { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string LocalizacaoFormatada { get; set; } = "";

        public double DistanciaKm { get; set; }
    }
}
