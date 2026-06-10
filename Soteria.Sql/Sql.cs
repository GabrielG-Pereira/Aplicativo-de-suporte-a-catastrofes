using Microsoft.Data.SqlClient;
using Soteria.Sql.Models;

namespace Soteria.Sql
{
    public static class Sql
    {
        private static readonly string connectionString =
            "Server=localhost;" +
            "Database=DBSQL_SOTERIA;" +
            "Trusted_Connection=True;" +
            "TrustServerCertificate=True;";

        public static T? ExecutarScalar<T>(string sql)
        {
            try
            {
                using SqlConnection conexao = new SqlConnection(connectionString);
                conexao.Open();

                using SqlCommand comando = new(sql, conexao);
                object resultado = comando.ExecuteScalar();

                // Caso venha null do banco
                if (resultado == null || resultado == DBNull.Value)
                    return default;

                return (T)Convert.ChangeType(resultado, typeof(T));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao executar consulta:");
                Console.WriteLine(ex.Message);
                return default;
            }
        }

        public static List<Demanda> ListarDemandas(Guid idPontoEvento)
        {
            List<Demanda> demandas = new();

            try
            {
                using SqlConnection conexao = new(connectionString);
                conexao.Open();

                string sql = @"
            SELECT
                d.id,
                d.id_ponto_evento,
                d.nome,
                d.descricao,
                d.status,
                d.categoria
            FROM demanda d
            WHERE d.id_ponto_evento = @id
            ORDER BY d.status";

                using SqlCommand comando = new(sql, conexao);
                comando.Parameters.AddWithValue("@id", idPontoEvento);

                using SqlDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    demandas.Add(new Demanda
                    {
                        Id = reader.GetGuid(0),
                        IdPontoEvento = reader.GetGuid(1),
                        Nome = reader.GetString(2),
                        Descricao = reader.IsDBNull(3) ? "" : reader.GetString(3),
                        Status = MapStatusDemanda(reader.GetString(4)),
                        Categoria = reader.GetGuid(5)
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao listar demandas:");
                Console.WriteLine(ex.Message);
            }

            return demandas;
        }

        public static void AdicionarDemanda(int id_ponto_evento, string nome, string descricao, StatusDemanda status)
        {
            ExecutarScalar<int>($"INSERT INTO demanda (id_ponto_evento, nome, descricao, status) VALUES ({id_ponto_evento}, '{nome}', '{descricao}', {status})");
        }

        public static List<Especialidade> ListarEspecialidades()
        {
            List<Especialidade> especialidades = new();

            try
            {
                using SqlConnection conexao = new(connectionString);
                conexao.Open();

                string sql = @"
                    SELECT
                        id,
                        nome
                    FROM especialidade_voluntario
                    ORDER BY nome";

                using SqlCommand comando = new(sql, conexao);

                using SqlDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    especialidades.Add(new Especialidade
                    {
                        Id = reader.GetGuid(0),
                        Nome = reader.GetString(1)
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao listar especialidades:");
                Console.WriteLine(ex.Message);
            }

            return especialidades;
        }

        public static string CadastrarVoluntario(string nome, string email, string telefone, string senha, string confirmarSenha, Guid especialidade, bool disponibilidade)
        {
            try
            {
                if (senha != confirmarSenha)
                    return "Erro";

                // Verifica se já existe email cadastrado
                int qtdEmail = ExecutarScalar<int>(
                    $"SELECT COUNT(*) FROM usuario WHERE email = '{email}'");

                if (qtdEmail > 0)
                    return "Erro";

                Guid idUsuario = Guid.NewGuid();
                Guid idVoluntario = Guid.NewGuid();

                // Cadastro do usuário
                ExecutarScalar<int>($@"
                    INSERT INTO usuario
                    (
                        id,
                        nome,
                        email,
                        senha,
                        contato,
                        tipo
                    )
                    VALUES
                    (
                        '{idUsuario}',
                        '{nome}',
                        '{email}',
                        '{senha}',
                        '{telefone}',
                        'Voluntario'
                    );

                    SELECT 1;
                ");

                // Cadastro do voluntário
                ExecutarScalar<int>($@"
                    INSERT INTO voluntario
                    (
                        id,
                        id_usuario,
                        especialidade,
                        localizacao,
                        disponivel
                    )
                    VALUES
                    (
                        '{idVoluntario}',
                        '{idUsuario}',
                        '{especialidade}',
                        geography::Point(0,0,4326),
                        {(disponibilidade ? 1 : 0)}
                    );

                    SELECT 1;
                ");

                return "Cadastrado com sucesso";
            }
            catch
            {
                return "Erro";
            }
        }

        public static ResultadoAutenticacao AutenticarVoluntario(string email, string senha)
        {
            ResultadoAutenticacao resultado = new();

            try
            {
                using SqlConnection conexao = new(connectionString);

                conexao.Open();

                string sql = @"
            SELECT
                u.id,
                v.id,
                u.senha
            FROM usuario u
            INNER JOIN voluntario v
                ON v.id_usuario = u.id
            WHERE u.email = @email
            AND u.tipo = 'Voluntario'";

                using SqlCommand comando =
                    new(sql, conexao);

                comando.Parameters.AddWithValue(
                    "@email",
                    email);

                using SqlDataReader reader =
                    comando.ExecuteReader();

                if (!reader.Read())
                {
                    resultado.Mensagem =
                        "Usuário não encontrado";

                    return resultado;
                }

                Guid idUsuario = reader.GetGuid(0);
                Guid idVoluntario = reader.GetGuid(1);
                string senhaBanco = reader.GetString(2);

                if (senhaBanco != senha)
                {
                    resultado.Mensagem =
                        "Senha incorreta";

                    return resultado;
                }

                Sessao.IdUsuario = idUsuario;
                Sessao.IdVoluntario = idVoluntario;

                resultado.Sucesso = true;
                resultado.IdUsuario = idUsuario;
                resultado.IdVoluntario = idVoluntario;

                return resultado;
            }
            catch (Exception ex)
            {
                resultado.Mensagem = ex.Message;
                return resultado;
            }
        }

        public static Evento? BuscarEventoPorId(Guid idEvento)
        {
            try
            {
                using SqlConnection conexao =
                    new(connectionString);

                conexao.Open();

                string sql = @"
            SELECT
                id,
                nome,
                descricao,
                status,
                data_inicio,
                localizacao.Lat,
                localizacao.Long
            FROM evento
            WHERE id = @id";

                using SqlCommand comando =
                    new(sql, conexao);

                comando.Parameters.AddWithValue(
                    "@id",
                    idEvento);

                using SqlDataReader reader =
                    comando.ExecuteReader();

                if (!reader.Read())
                    return null;

                return new Evento
                {
                    Id = reader.GetGuid(0),
                    Nome = reader.GetString(1),
                    Descricao = reader.IsDBNull(2)
                        ? ""
                        : reader.GetString(2),
                    Status = reader.GetString(3),
                    DataInicio = reader.GetDateTime(4),
                    Latitude = reader.GetDouble(5),
                    Longitude = reader.GetDouble(6),
                    LocalizacaoFormatada =
                        $"{reader.GetDouble(5):F6}, {reader.GetDouble(6):F6}"
                };
            }
            catch
            {
                return null;
            }
        }

        public static List<Evento> ListarEventosParaVoluntario(Guid idVoluntario)
        {
            List<Evento> eventos = new();

            try
            {
                using SqlConnection conexao = new(connectionString);

                conexao.Open();

                string sql = @"
            SELECT
                e.id,
                e.nome,
                e.descricao,
                e.status,
                e.data_inicio,
                e.localizacao.Lat AS Latitude,
                e.localizacao.Long AS Longitude,
                v.localizacao.STDistance(e.localizacao) / 1000.0 AS DistanciaKm
            FROM evento e
            INNER JOIN voluntario v
                ON v.id = @idVoluntario
            WHERE e.status = 'Ativo'
            ORDER BY DistanciaKm";

                using SqlCommand comando = new(sql, conexao);

                comando.Parameters.AddWithValue(
                    "@idVoluntario",
                    idVoluntario);

                using SqlDataReader reader =
                    comando.ExecuteReader();

                while (reader.Read())
                {
                    eventos.Add(new Evento
                    {
                        Id = reader.GetGuid(0),
                        Nome = reader.GetString(1),
                        Descricao = reader.IsDBNull(2)
                            ? ""
                            : reader.GetString(2),
                        Status = reader.GetString(3),
                        DataInicio = reader.GetDateTime(4),
                        Latitude = reader.GetDouble(5),
                        Longitude = reader.GetDouble(6),
                        DistanciaKm = reader.IsDBNull(7)
                            ? 0
                            : reader.GetDouble(7),
                        LocalizacaoFormatada =
                            $"{reader.GetDouble(5):F6}, {reader.GetDouble(6):F6}"
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao listar eventos:");
                Console.WriteLine(ex.Message);
            }

            return eventos;
        }

        public static List<Evento> ListarEventosInscritos(Guid idVoluntario)
        {
            List<Evento> eventos = new();

            using SqlConnection conexao = new(connectionString);

            conexao.Open();

            string sql = @"
        SELECT
            e.id,
            e.nome,
            e.descricao,
            e.status,
            e.data_inicio,
            e.localizacao.Lat,
            e.localizacao.Long
        FROM voluntario_evento ve
        INNER JOIN evento e
            ON e.id = ve.id_evento
        WHERE ve.id_voluntario = @idVoluntario
        ORDER BY ve.data_aceito DESC";

            using SqlCommand comando =
                new(sql, conexao);

            comando.Parameters.AddWithValue(
                "@idVoluntario",
                idVoluntario);

            using SqlDataReader reader =
                comando.ExecuteReader();

            while (reader.Read())
            {
                eventos.Add(new Evento
                {
                    Id = reader.GetGuid(0),
                    Nome = reader.GetString(1),
                    Descricao = reader.IsDBNull(2)
                        ? ""
                        : reader.GetString(2),
                    Status = reader.GetString(3),
                    DataInicio = reader.GetDateTime(4),
                    Latitude = reader.GetDouble(5),
                    Longitude = reader.GetDouble(6)
                });
            }

            return eventos;
        }

        public static string InscreverNoEvento(Guid idEvento, Guid idVoluntario)
        {
            try
            {
                string? statusEvento =
                    ExecutarScalar<string>(
                        $"SELECT status FROM evento WHERE id = '{idEvento}'");

                if (string.IsNullOrWhiteSpace(statusEvento))
                    return "Evento não encontrado";

                if (statusEvento == "Encerrado")
                    return "Evento encerrado";

                if (statusEvento == "Alerta Crítico")
                    return "Evento indisponível";

                int jaExiste =
                    ExecutarScalar<int>(
                        $@"
                SELECT COUNT(*)
                FROM voluntario_evento
                WHERE id_evento = '{idEvento}'
                AND id_voluntario = '{idVoluntario}'
                ");

                if (jaExiste > 0)
                    return "Voluntário já inscrito";

                ExecutarScalar<int>(
                    $@"
            INSERT INTO voluntario_evento
            (
                id,
                id_evento,
                id_voluntario,
                status,
                data_aceito
            )
            VALUES
            (
                '{Guid.NewGuid()}',
                '{idEvento}',
                '{idVoluntario}',
                'A caminho',
                GETDATE()
            );

            SELECT 1;
            ");

                return "Sucesso";
            }
            catch
            {
                return "Erro";
            }
        }

        public static Guid ObterVoluntarioLogado()
        {
            return Sessao.IdVoluntario;
        }

        public static void Logout()
        {
            Sessao.IdUsuario = Guid.Empty;
            Sessao.IdVoluntario = Guid.Empty;
        }

        public static Evento? BuscarEventoInscritoAtual(Guid idVoluntario)
        {
            try
            {
                using SqlConnection conexao =
                    new(connectionString);

                conexao.Open();

                string sql = @"
            SELECT TOP 1
                e.id,
                e.nome,
                e.descricao,
                e.status,
                e.data_inicio,
                e.localizacao.Lat,
                e.localizacao.Long,
                ve.status,
                v.localizacao.STDistance(e.localizacao) / 1000.0
            FROM voluntario_evento ve
            INNER JOIN evento e
                ON e.id = ve.id_evento
            INNER JOIN voluntario v
                ON v.id = ve.id_voluntario
            WHERE ve.id_voluntario = @idVoluntario
            AND ve.status IN ('A caminho','No local')
            ORDER BY ve.data_aceito DESC";

                using SqlCommand comando =
                    new(sql, conexao);

                comando.Parameters.AddWithValue(
                    "@idVoluntario",
                    idVoluntario);

                using SqlDataReader reader =
                    comando.ExecuteReader();

                if (!reader.Read())
                    return null;

                return new Evento
                {
                    Id = reader.GetGuid(0),
                    Nome = reader.GetString(1),
                    Descricao = reader.IsDBNull(2)
                        ? ""
                        : reader.GetString(2),
                    Status = reader.GetString(3),
                    DataInicio = reader.GetDateTime(4),
                    Latitude = reader.GetDouble(5),
                    Longitude = reader.GetDouble(6),
                    DistanciaKm = reader.IsDBNull(8)
                        ? 0
                        : reader.GetDouble(8)
                };
            }
            catch
            {
                return null;
            }
        }

        public static bool ConfirmarChegadaEvento(Guid idVoluntario)
        {
            try
            {
                ExecutarScalar<int>(
                    $@"
            UPDATE TOP (1) voluntario_evento
            SET
                status = 'No local',
                data_checkin = GETDATE()
            WHERE id_voluntario = '{idVoluntario}'
            AND status = 'A caminho';

            SELECT 1;
            ");

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool CancelarInscricaoEvento(Guid idVoluntario)
        {
            try
            {
                ExecutarScalar<int>(
                    $@"
            UPDATE TOP (1) voluntario_evento
            SET status = 'Cancelado'
            WHERE id_voluntario = '{idVoluntario}'
            AND status IN ('A caminho','No local');

            SELECT 1;
            ");

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static PerfilVoluntario? BuscarPerfilVoluntario(Guid idVoluntario)
        {
            try
            {
                using SqlConnection conexao =
                    new(connectionString);

                conexao.Open();

                string sql = @"
            SELECT
                v.id,
                u.nome,
                u.email,
                u.contato,
                ev.nome,
                v.disponivel,
                v.localizacao.Lat,
                v.localizacao.Long
            FROM voluntario v
            INNER JOIN usuario u
                ON u.id = v.id_usuario
            INNER JOIN especialidade_voluntario ev
                ON ev.id = v.especialidade
            WHERE v.id = @id";

                using SqlCommand comando =
                    new(sql, conexao);

                comando.Parameters.AddWithValue(
                    "@id",
                    idVoluntario);

                using SqlDataReader reader =
                    comando.ExecuteReader();

                if (!reader.Read())
                    return null;

                return new PerfilVoluntario
                {
                    Id = reader.GetGuid(0),
                    Nome = reader.GetString(1),
                    Email = reader.GetString(2),
                    Contato = reader.GetString(3),
                    Especialidade = reader.GetString(4),
                    Disponivel = reader.GetBoolean(5),
                    Latitude = reader.GetDouble(6),
                    Longitude = reader.GetDouble(7)
                };
            }
            catch
            {
                return null;
            }
        }
        private static StatusDemanda MapStatusDemanda(string status)
        {
            return status switch
            {
                "Crítico" => StatusDemanda.Critico,
                "Médio" => StatusDemanda.Medio,
                "Estável" => StatusDemanda.Estavel,
                _ => StatusDemanda.Medio
            };
        }

        public static PontoApoio? BuscarPontoApoio(Guid idPonto)
        {
            try
            {
                using SqlConnection conexao = new(connectionString);
                conexao.Open();

                string sql = @"
            SELECT id, nome, endereco, contato,
                   CASE WHEN temporario = 1 THEN 'Temporário' ELSE 'Fixo' END
            FROM ponto_coleta
            WHERE id = @id";

                using SqlCommand cmd = new(sql, conexao);
                cmd.Parameters.AddWithValue("@id", idPonto);

                using SqlDataReader r = cmd.ExecuteReader();

                if (!r.Read()) return null;

                return new PontoApoio
                {
                    Id = r.GetGuid(0),
                    Nome = r.GetString(1),
                    Endereco = r.GetString(2),
                    Contato = r.GetString(3),
                    Tipo = r.GetString(4)
                };
            }
            catch
            {
                return null;
            }
        }

        public static List<PontoApoio> ListarPontosProximos(Guid idVoluntario)
        {
            List<PontoApoio> pontos = new();

            try
            {
                using SqlConnection conexao = new(connectionString);
                conexao.Open();

                string sql = @"
            SELECT
                p.id,
                p.nome,
                p.endereco,
                p.contato,
                CASE WHEN p.temporario = 1 THEN 'Temporário' ELSE 'Fixo' END AS tipo,
                v.localizacao.STDistance(p.localizacao) / 1000.0 AS distanciaKm,
                p.localizacao.Lat,
                p.localizacao.Long
            FROM ponto_coleta p
            CROSS JOIN voluntario v
            WHERE v.id = @idVoluntario
            ORDER BY distanciaKm";

                using SqlCommand cmd = new(sql, conexao);
                cmd.Parameters.AddWithValue("@idVoluntario", idVoluntario);

                using SqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    pontos.Add(new PontoApoio
                    {
                        Id = r.GetGuid(0),
                        Nome = r.GetString(1),
                        Endereco = r.GetString(2),
                        Contato = r.GetString(3),
                        Tipo = r.GetString(4),
                        DistanciaKm = r.IsDBNull(5) ? 0 : r.GetDouble(5),
                        Latitude = r.GetDouble(6),
                        Longitude = r.GetDouble(7)
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return pontos;
        }

        public static PontoApoio? BuscarPontoPorId(Guid id)
        {
            try
            {
                using SqlConnection conexao = new(connectionString);
                conexao.Open();

                string sql = @"
            SELECT
                id,
                nome,
                endereco,
                contato,
                CASE WHEN temporario = 1 THEN 'Temporário' ELSE 'Fixo' END AS tipo,
                localizacao.Lat,
                localizacao.Long
            FROM ponto_coleta
            WHERE id = @id";

                using SqlCommand cmd = new(sql, conexao);
                cmd.Parameters.AddWithValue("@id", id);

                using SqlDataReader r = cmd.ExecuteReader();

                if (!r.Read())
                    return null;

                return new PontoApoio
                {
                    Id = r.GetGuid(0),
                    Nome = r.GetString(1),
                    Endereco = r.GetString(2),
                    Contato = r.GetString(3),
                    Tipo = r.GetString(4),
                    Latitude = r.GetDouble(5),
                    Longitude = r.GetDouble(6)
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static List<CategoriaDemanda> ListarCategoriasDemanda()
        {
            List<CategoriaDemanda> categorias = new();

            try
            {
                using SqlConnection conexao = new(connectionString);
                conexao.Open();

                string sql = @"SELECT id, nome FROM categoria_demanda ORDER BY nome";

                using SqlCommand cmd = new(sql, conexao);
                using SqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    categorias.Add(new CategoriaDemanda
                    {
                        Id = r.GetGuid(0),
                        Nome = r.GetString(1)
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return categorias;
        }

        public static string AdicionarDemanda(Guid idPontoEvento, Guid idCategoria, string descricao, StatusDemanda status)
        {
            try
            {
                ExecutarScalar<int>($@"
            INSERT INTO demanda
            (
                id,
                id_ponto_evento,
                categoria,
                descricao,
                status
            )
            VALUES
            (
                '{Guid.NewGuid()}',
                '{idPontoEvento}',
                '{idCategoria}',
                '{descricao}',
                '{status}'
            );

            SELECT 1;
        ");

                return "Sucesso";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Erro";
            }
        }

        // -- SOTERIA.ADMIN --


        // PAGINA DE LOGIN
        public static UsuarioSessao? ValidarLogin(string email, string senha){
            try{
                using SqlConnection conexao = new(connectionString);
                conexao.Open();

                // Busca o usuário pelo email e senha
                string sql = "SELECT id, nome, email, tipo FROM usuario WHERE email = @email AND senha = @senha";

                using SqlCommand comando = new(sql, conexao);
                comando.Parameters.AddWithValue("@email", email);
                comando.Parameters.AddWithValue("@senha", senha); // Em produção, use Hash de senha!

                using SqlDataReader reader = comando.ExecuteReader();

                if (reader.Read())
                {
                    return new UsuarioSessao
                    {
                        Id = reader.GetGuid(0),
                        Nome = reader.GetString(1),
                        Email = reader.GetString(2),
                        Tipo = reader.GetString(3)
                    };
                }
            }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao validar login: " + ex.Message);
        }

        return null; // Usuário não encontrado ou erro
    }

        

        public static List<EventoLista> ListarEventos()
        {
            List<EventoLista> lista = new();
            try
            {
                using SqlConnection conexao = new(connectionString);
                conexao.Open();

                // Buscamos apenas o que a tela precisa
                string sql = "SELECT id, nome, status FROM evento ORDER BY data_inicio DESC";

                using SqlCommand comando = new(sql, conexao);
                using SqlDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new EventoLista
                    {
                        Id = reader.GetGuid(0),
                        Nome = reader.GetString(1),
                        Status = reader.GetString(2)
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao listar eventos: " + ex.Message);
            }
            return lista;
        }

        public static bool InserirEvento(string nome, string descricao, string status)
        {
            try
            {
                using SqlConnection conexao = new(connectionString);
                conexao.Open();

                // Nota: localizacao é obrigatória (NOT NULL), então inserimos um ponto padrão (0,0)
                string sql = @"
            INSERT INTO dbo.evento (nome, descricao, localizacao, status, data_inicio)
            VALUES (@nome, @descricao, geography::Point(0, 0, 4326), @status, GETDATE())";

                using SqlCommand comando = new(sql, conexao);
                comando.Parameters.AddWithValue("@nome", nome);
                comando.Parameters.AddWithValue("@descricao", descricao);
                comando.Parameters.AddWithValue("@status", status);

                int linhasAfetadas = comando.ExecuteNonQuery();
                return linhasAfetadas > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao inserir evento: " + ex.Message);
                return false;
            }
        }


        public static EventoLista? BuscarEventoPorIdSoteriaAdmin(Guid id)
        {
            try
            {
                using SqlConnection conexao = new(connectionString);
                conexao.Open();
                string sql = "SELECT id, nome, descricao, status FROM evento WHERE id = @id";
                using SqlCommand comando = new(sql, conexao);
                comando.Parameters.AddWithValue("@id", id);
                using SqlDataReader reader = comando.ExecuteReader();

                if (reader.Read())
                {
                    return new EventoLista
                    {
                        Id = reader.GetGuid(0),
                        Nome = reader.GetString(1),
                        // Aqui tratamos a descrição (que pode conter o endereço do cadastro anterior)
                        Descricao = reader.IsDBNull(2) ? "" : reader.GetString(2),
                        Status = reader.GetString(3)
                    };
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return null;
        }

        public static bool AtualizarEvento(Guid id, string nome, string descricao, string status)
        {
            try
            {
                using SqlConnection conexao = new(connectionString);
                conexao.Open();
                string sql = "UPDATE evento SET nome = @nome, descricao = @descricao, status = @status WHERE id = @id";
                using SqlCommand comando = new(sql, conexao);
                comando.Parameters.AddWithValue("@id", id);
                comando.Parameters.AddWithValue("@nome", nome);
                comando.Parameters.AddWithValue("@descricao", descricao);
                comando.Parameters.AddWithValue("@status", status);
                return comando.ExecuteNonQuery() > 0;
            }
            catch { return false; }
        }

        public static bool DeletarEvento(Guid id)
        {
            try
            {
                using SqlConnection conexao = new(connectionString);
                conexao.Open();
                string sql = "DELETE FROM evento WHERE id = @id";
                using SqlCommand comando = new(sql, conexao);
                comando.Parameters.AddWithValue("@id", id);
                return comando.ExecuteNonQuery() > 0;
            }
            catch { return false; }
        }



    }
}
