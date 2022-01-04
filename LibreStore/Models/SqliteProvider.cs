using Microsoft.Data.Sqlite;
using LibreStore.Models;
public class SqliteProvider : IPersistable{

    private SqliteConnection connection;
    public SqliteCommand command{get;set;}
    
    private String [] allTableCreation = {
                @"CREATE TABLE IF NOT EXISTS [MainToken]
                (
                    [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    [OwnerId] INTEGER NOT NULL default(0),
                    [Key] NVARCHAR(2048)  NOT NULL UNIQUE,
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
                    [MainTokenId] INTEGER NOT NULL default(0),
                    [IpAddress] NVARCHAR(60),
                    [Action] NVARCHAR(75),
                    [Created] NVARCHAR(30) default (datetime('now','localtime')),
                    [Active] BOOLEAN default (1)
                )
                "};

    public SqliteProvider()
    {
        try{
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
                }
                Console.WriteLine(connection.DataSource);
        }
        finally{
            if (connection != null){
                connection.Close();
            }
        }
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
                    var id = reader.GetInt32(0);
                    var ownerId = reader.GetInt32(1);
                    var key = reader.GetString(2);
                    var created = reader.GetString(3);
                    var active = reader.GetInt16(4);
                    allTokens.Add(new MainToken(id,key,DateTime.Parse(created),ownerId,Convert.ToBoolean(active)));
                    Console.WriteLine($"key: {key}!");
                }
            }
            return allTokens;
        }
        finally{
            if (connection != null){
                connection.Close();
            }
        }
    }

    public int GetOrInsert(){
        try{
            Console.WriteLine("GetOrInsert...");
            connection.Open();
            Console.WriteLine("Opening...");
            using (var reader = command.ExecuteReader())
            {
                reader.Read();
                var id = reader.GetInt32(0);
                Console.WriteLine($"GetOrInsert() id: {id}");
                reader.Close();
                return id;
            }
        }
        finally{
            if (connection != null){
                connection.Close();
            }
        }
    }
    
    public int Save(){
        
        try{
            Console.WriteLine("Saving...");
            connection.Open();
            Console.WriteLine("Opened.");
            command.ExecuteNonQuery();
            Console.WriteLine("inserted.");
            return 0;
        }
        finally{
            if (connection != null){
                connection.Close();
            }
        }
    }
}