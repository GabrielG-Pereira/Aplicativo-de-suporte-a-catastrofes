using System;
using System.Collections.Generic;
using System.Text;

namespace Soteria.Sql.Models
{
    public static class Sessao
    {
        public static Guid IdUsuario { get; set; }

        public static Guid IdVoluntario { get; set; }

        public static bool Logado =>
            IdUsuario != Guid.Empty;
    }
}
