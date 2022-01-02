using Microsoft.Data.Sqlite;
using LibreStore.Models;
class SqliteProvider : IPersistable{

    private SqliteConnection connection;
    private SqliteCommand command;
    private String commandText;

    private String createTableQuery = @"CREATE TABLE IF NOT EXISTS [MainToken]
                (
                [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                [Key] NVARCHAR(2048)  NULL
                )";

    public SqliteProvider(String commandText)
    {
        connection = new SqliteConnection("Data Source=librestore.db");
        // ########### FYI THE DB is created when it is OPENED ########
        connection.Open();
        command = connection.CreateCommand();
        FileInfo fi = new FileInfo("librestore.db");
        if (fi.Length == 0){
            command.CommandText = createTableQuery;
            
            command.ExecuteNonQuery();
            connection.Close();
        }
        Console.WriteLine(connection.DataSource);
        
        command.CommandText = commandText;
        
    }

    public void ConfigInsert(MainToken mt){
        command.Parameters.AddWithValue("$key",mt.Key);
    }
    public int Save(){
        Console.WriteLine("Saving...");
        connection.Open();
        Console.WriteLine("Opening...");
        command.ExecuteNonQuery();
        Console.WriteLine("inserting...");
        // using (var reader = command.ExecuteReader())
        // {
        //     while (reader.Read())
        //     {
        //         var name = reader.GetString(0);

        //         Console.WriteLine($"Hello, {name}!");
        //     }
        // }

            return 0;
    }
}