using MySql.Data.MySqlClient;

string connectString = "server=localhost;userid=root;password=;database=habittracker";
using (var connection = new MySqlConnection(connectString))
{
    connection.Open();
    var tableCmd = connection.CreateCommand();
    tableCmd.CommandText =
        @"CREATE TABLE IF NOT EXISTS agua (
        id INTEGER PRIMARY KEY AUTO_INCREMENT,
        date TEXT,
        quantity INTEGER)";
    tableCmd.ExecuteNonQuery();
    connection.Close();
}

static void GetInput()
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
                Update();
                break;
            default:
                Console.WriteLine("\nComando inválido, digite um número entre 0 e 4.\n");
                break;
        }
    }
}