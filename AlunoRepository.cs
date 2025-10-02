using System.Data;
using Microsoft.Data.SqlClient;
/// <summary>
/// Classe de repositório para gerenciar entidades Aluno no banco de dados.
/// </summary>
public class AlunoRepository : IRepository<Aluno>
{
    public string ConnectionString { get; set; }

    public AlunoRepository(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public void GarantirEsquema()
    {
        const string ddl = @"
       IF OBJECT_ID('dbo.Alunos', 'U') IS NULL
       BEGIN
           CREATE TABLE dbo.Alunos (
               Id INT IDENTITY(1,1) PRIMARY KEY,
               Nome NVARCHAR(100) NOT NULL,
               Idade INT NOT NULL,
               Email NVARCHAR(100) NOT NULL,
               DataNascimento DATE NOT NULL
           );
       END";
        using var conn = new SqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand(ddl, conn);
        cmd.ExecuteNonQuery();
    }

    public int Inserir(string nome, int idade, string email, DateTime dataNascimento)
    {
        const string sql = @"
       INSERT INTO Alunos (Nome, Idade, Email, DataNascimento)
       VALUES (@Nome, @Idade, @Email, @DataNascimento);
       SELECT SCOPE_IDENTITY();";
        using var conn = new SqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Nome", nome);
        cmd.Parameters.AddWithValue("@Idade", idade);
        cmd.Parameters.AddWithValue("@Email", email);
        cmd.Parameters.AddWithValue("@DataNascimento", dataNascimento);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public List<Aluno> Listar()
    {
        const string sql = "SELECT Id, Nome, Idade, Email, DataNascimento FROM Alunos";
        var alunos = new List<Aluno>();
        using var conn = new SqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            alunos.Add(new Aluno
            {
                Id = reader.GetInt32(0),
                Nome = reader.GetString(1),
                Idade = reader.GetInt32(2),
                Email = reader.GetString(3),
                DataNascimento = reader.GetDateTime(4)
            });
        }
        return alunos;
    }

    public int Atualizar(int id, string nome, int idade, string email, DateTime dataNascimento)
    {
        const string sql = @"
       UPDATE Alunos
       SET Nome = @Nome, Idade = @Idade, Email = @Email, DataNascimento = @DataNascimento
       WHERE Id = @Id";
        using var conn = new SqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@Nome", nome);
        cmd.Parameters.AddWithValue("@Idade", idade);
        cmd.Parameters.AddWithValue("@Email", email);
        cmd.Parameters.AddWithValue("@DataNascimento", dataNascimento);
        return cmd.ExecuteNonQuery();
    }

    public int Excluir(int id)
    {
        const string sql = "DELETE FROM Alunos WHERE Id = @Id";
        using var conn = new SqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", id);
        return cmd.ExecuteNonQuery();
    }
    public List<Aluno> Buscar(string propriedade, object valor)
    {
        var alunos = new List<Aluno>();
        var sql = $"SELECT Id, Nome, Idade, Email, DataNascimento FROM Alunos WHERE {propriedade} = @Valor";
        using var conn = new SqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Valor", valor);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            alunos.Add(new Aluno
            {
                Id = reader.GetInt32(0),
                Nome = reader.GetString(1),
                Idade = reader.GetInt32(2),
                Email = reader.GetString(3),
                DataNascimento = reader.GetDateTime(4)
            });
        }
        return alunos;
    }
}