using MySql.Data.MySqlClient;




namespace appCrudPessoa
{
    public class Progam
    {
        private static string connectionString = "Server=sql10.freesqldatabase.com;Database=sql10727350;Uid=sql10727350;Pwd=3y6BlDigUL;";
        private static List<Pessoa> pessoas = new List<Pessoa>();
        public static int proximoId = 1;

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine(" 1 - Adicionar Pessoa");
                Console.WriteLine(" 2 - Listar Pessoas");
                Console.WriteLine(" 3 - Editar Pessoa");
                Console.WriteLine(" 4 - Excluir Pessoa");
                Console.WriteLine(" 5 - Sair");
                Console.Write("Esolha uma opção a cima:");

                string opcao = Console.ReadLine();
                switch (opcao)
                {
                    case "1":
                        AdicionarPessoa();
                        break;

                    case "2":
                        ListarPessoa();
                        break;

                    case "3":
                        Editar();
                       
                        break;

                    case "4":
                        Exclui();
                        break;

                    case "5":
                        Console.Write(5);
                        return;
                        break;

                    default:
                        Console.WriteLine("Opção invalida!");
                        break;
                }
            }
        }
        static void AdicionarPessoa()
        {
            Console.WriteLine("Informe o Nome:");
            string nome = Console.ReadLine();

            Console.WriteLine("Informe o Email:");
            string email = Console.ReadLine();

            Console.WriteLine("Informe a Idade:");
            int idade = int.Parse(Console.ReadLine());

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO pessoa (nome,email,idade)VALUES(@Nome,@Email,@Idade)";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@Nome", nome);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Idade", idade);
                cmd.ExecuteNonQuery();
            }
            Console.WriteLine("Pessoa cadastrada om sucesso");

        }
        static void ListarPessoa()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id,Nome,Idade,Email FROM pessoa";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"Id: {reader["Id"]}, Nome: {reader["Nome"]},Idade: {reader["Idade"]},Email: {reader["Email"]}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Não existe pessoa cadastrada");
                    }
                }
            }



        }

        static void Exclui()
        {
            Console.WriteLine("Informe o Id da pessoa que deseja excluir: ");
            int idExclusao = int.Parse(Console.ReadLine());
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM pessoa Where Id =@Id";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@Id", idExclusao);
                int linhaAfetada = cmd.ExecuteNonQuery();

                if (linhaAfetada > 0)
                {
                    Console.WriteLine("Pessoa excluida com sucesso!");

                }
                else
                {
                    Console.WriteLine("Pessoa não encontrada");
                }
            }
        }

        static void Editar()
        {
            Console.WriteLine("Informe o Id da pessoa que deseja eitar: ");
            int idEditar = int.Parse(Console.ReadLine());
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM pessoa Where Id =@Id";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@Id", idEditar);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Console.WriteLine("Infore o novo nome: (*Deixe o campo em branco, para não alterar");
                        string novoNome = Console.ReadLine();
                        Console.WriteLine("Infore o novo email: (*Deixe o campo em branco, para não alterar");
                        string novoEmail = Console.ReadLine();
                        Console.WriteLine("Infore a nova idade: (*Deixe o campo em branco, para não alterar");
                        string novoIdade = Console.ReadLine();

                        reader.Close();
                        string queryUpdate = "UPDATE pessoa SET Nome = @Nome,Email = @Email, Idade = @Idade Where Id =@Id";
                        cmd = new MySqlCommand(queryUpdate, connection);
                        cmd.Parameters.AddWithValue("@Nome", string.IsNullOrEmpty(novoNome) ? reader["Nome"] : novoNome);
                        cmd.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(novoEmail) ? reader["Email"] : novoEmail);
                        cmd.Parameters.AddWithValue("@Idade", string.IsNullOrEmpty(novoIdade) ? reader["Idade"] : int.Parse(novoIdade));
                        cmd.Parameters.AddWithValue("@Id", idEditar);

                        cmd.ExecuteNonQuery();
                        Console.WriteLine("A Pessoa foi atualizada com sucesso!");


                    }

                    else
                    {
                        Console.WriteLine("O Id da Pessoa informada não existe!");
                    }
                }
            }
        }
    }
}