using MySql.Data.MySqlClient;
using System.Globalization;

internal class Program
{
    static string connectString = "server=localhost;userid=root;password=;database=habittracker";
    private static void Main(string[] args)
    {

        using (var connection = new MySqlConnection(connectString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS agua (
        id INTEGER PRIMARY KEY AUTO_INCREMENT,
        date DATETIME,
        quantity INTEGER)";
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }

        GetInput();

    }

    private static void GetInput()
    {
        Console.Clear();
        bool close = false;
        while (close == false)
        {
            Console.WriteLine("\n\nMENU PRINCIPAL");
            Console.WriteLine("\nO que você gostaria de fazer?");
            Console.WriteLine("\nDigite 0 para encerrar o aplicativo");
            Console.WriteLine("Digite 1 para ver os dados salvos");
            Console.WriteLine("Digite 2 para inserir novos dados");
            Console.WriteLine("Digite 3 para apagar dados");
            Console.WriteLine("Digite 4 para atualizar dados");
            Console.WriteLine("-----------------------------------------\n");

            string input = Console.ReadLine();

            switch (input)
            {
                case "0":
                    Console.WriteLine("\nTchau!\n");
                    close = true;
                    break;
                case "1":
                    GetAllRecords();
                    break;
                case "2":
                    Insert();
                    break;
                case "3":
                    Delete();
                    break;
                case "4":
                  //  Update();
                    break;
                default:
                    Console.WriteLine("\nComando inválido, digite um número entre 0 e 4.\n");
                    break;
            }
        }
    }

    private static void Insert()
    {
        string date = GetDateInput();
        int quant = GetNumberInput("\n\nPor favor, informe a quantia de água utilizando números inteiros.\n\n");

        using (var connection = new MySqlConnection(connectString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"INSERT INTO agua(date, quantity) VALUES ('{date}', {quant})";
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    private static void Delete()
    {
        Console.Clear();
        GetAllRecords();

        var recordId = GetNumberInput("\n\nPor favor, informe o Id do dado que você deseja apagar. Digite 0 para voltar ao menu inicial. \n\n");
        using (var connection = new MySqlConnection(connectString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE from agua WHERE Id = '{recordId}'";
            int rowCount = tableCmd.ExecuteNonQuery();
            if (rowCount == 0)
            {
                Console.WriteLine($"\n\nDado com o Id {recordId} não existe. \n\n");
                Delete();
            }
        }
        Console.WriteLine($"\n\nDado com o Id {recordId} foi apagadado.Aperte ENTER para continuar.");
        Console.ReadLine();
        GetInput();
    }

    private static string GetDateInput()
    {
        Console.WriteLine("\n\nPor favor, informe a data (dd/mm/aa). Digite 0 para voltar ao menu inicial.\n\n");
        string dateInput = Console.ReadLine();
        if (dateInput == "0") GetInput();
        return dateInput;
    }

    private static int GetNumberInput(string message)
    {
        Console.WriteLine(message);
        string numberInput = Console.ReadLine();
        if (numberInput == "0") GetInput();
        int finalInput = Convert.ToInt32(numberInput);
        return finalInput;
    }

    private static void GetAllRecords()
    {
        Console.Clear();
        using (var connection = new MySqlConnection(connectString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"SELECT * FROM agua";

            List<DrinkingWater> tableData = new();
            MySqlDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(new DrinkingWater
                    {
                        Id = reader.GetInt32(0),
                        Date = reader.GetDateTime(1),
                        Quantity = reader.GetInt32(2)
                    }); ;
                }
            }
            else
            {
                Console.WriteLine("Sem dados encontrados.");
            }
            connection.Close();
            Console.WriteLine("---------------------------\n");
            foreach (var dw in tableData)
            {
                Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MM-yyyy")} - Quantidade: {dw.Quantity}");
            }
            Console.WriteLine("----------------------------\n");
        }
    }
}

internal class DrinkingWater
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}