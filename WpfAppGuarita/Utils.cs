using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Documents;

namespace WpfAppGuarita
{
    public class ContatoModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public byte Esconder { get; set; }

        public ContatoModel(int id, string nome, string email, byte esconder)
        {
            Id = id;
            Nome = nome;
            Email = email;
            Esconder = esconder;
        }

    }

    public class Banco
    {
        public SqlConnection Conexao { get; private set; }

        public Banco()
        {
            string rotaBanco = "Server=(localdb)\\mssqllocaldb;" +
                               "Database=DbContatos;" +
                               "Trusted_Connection=True;" +
                               "MultipleActiveResultSets=true";

            Conexao = new SqlConnection(rotaBanco);

            try
            {
                Conexao.Open();
                Console.WriteLine("✅ Connected to SQL Server successfully.");
                Console.WriteLine(Conexao.Database);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("❌ SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ General Error: " + ex.Message);
            }
        }

        //public SqlDataReader ViewBanco()
        //{
        //    using (SqlCommand command = new SqlCommand("SELECT * FROM Contato", Conexao))
        //    {
        //        return command.ExecuteReader(CommandBehavior.CloseConnection);
        //    }
        //}

        public List<ContatoModel> ViewBanco()
        {
            using (SqlCommand command = new SqlCommand("SELECT * FROM Contato", Conexao))
            {
                List<ContatoModel> lista = new List<ContatoModel>();

                SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    lista.Add(
                        new ContatoModel(
                            (int)reader["Id"],
                            (string)reader["Email"],
                            (string)reader["Email"],
                            (byte)reader["Esconder"]
                            )
                        );
                }

                return lista;
            }
        }

    }

}
