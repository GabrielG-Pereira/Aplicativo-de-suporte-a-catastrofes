using System;
using System.Collections.Generic;
using System.Text;

namespace Soteria.Sql.Models
{
    public class ResultadoAutenticacao
    {
        public bool Sucesso { get; set; }

        public string Mensagem { get; set; } = "";

        public Guid IdUsuario { get; set; }

        public Guid IdVoluntario { get; set; }
    }
}
