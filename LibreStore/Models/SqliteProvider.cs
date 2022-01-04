using Microsoft.Data.Sqlite;
using LibreStore.Models;
public class SqliteProvider : IPersistable{

    private SqliteConnection connection;
    public SqliteCommand command{get;set;}
    
    private String [] allTableCreation = {
                @"CREATE TABLE IF NOT EXISTS [MainToken]
                (
                    [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    [Key] NVARCHAR(2048)  NOT NULL,
                    [Created] NVARCHAR(30) default (datetime('now','localtime')),
                    [Active] BOOLEAN default (1)
                )",

                @"CREATE TABLE IF NOT EXISTS [Data]
                (
                    [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    [MainTokenId] INTEGER NOT NULL,
                    [Data] NVARCHAR(65535),
                    [Created] NVARCHAR(30) default (datetime('now','localtime')),
                    [Updated] NVARCHAR(30) ,
                    [Active] BOOLEAN default(1)
                )",
                @"CREATE TABLE IF NOT EXISTS [Usage]
                (
                    [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    [IpAddress] NVARCHAR(60),
                    [Action] NVARCHAR(75),
                    [Created] NVARCHAR(30) default (datetime('now','localtime')),
                    [Active] BOOLEAN default (1)
                )
                "};

    public SqliteProvider()
    {
        connection = new SqliteConnection("Data Source=librestore.db");
        // ########### FYI THE DB is created when it is OPENED ########
        connection.Open();
        command = connection.CreateCommand();
        FileInfo fi = new FileInfo("librestore.db");
        if (fi.Length == 0){
            foreach (String tableCreate in allTableCreation){
                command.CommandText = tableCreate;
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
        Console.WriteLine(connection.DataSource);
        
    }

    public List<MainToken> GetAllTokens(){
        command.CommandText = "Select * from MainToken";
        List<MainToken> allTokens = new List<MainToken>();
        try{
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var key = reader.GetString(1);
                    var created = reader.GetString(2);
                    var active = reader.GetString(3);
                    allTokens.Add(new MainToken(key));
                    Console.WriteLine($"key: {key}!");
                }
            }
            return allTokens;
        }
        finally{
            connection.Close();
        }
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