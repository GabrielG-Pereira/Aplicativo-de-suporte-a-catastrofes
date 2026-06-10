using System;
using System.Collections.Generic;
using System.Text;

namespace Soteria.Sql.Models
{
    public class PerfilVoluntario
    {
        public Guid Id { get; set; }

        public string Nome { get; set; } = "";

        public string Email { get; set; } = "";

        public string Contato { get; set; } = "";

        public string Especialidade { get; set; } = "";

        public bool Disponivel { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Endereco =>
            $"{Latitude:F6}, {Longitude:F6}";
    }
}
