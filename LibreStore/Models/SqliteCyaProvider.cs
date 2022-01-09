using Microsoft.Data.Sqlite;
using LibreStore.Models;
public class SqliteCyaProvider : IPersistable{

    private SqliteConnection connection;
    public SqliteCommand command{get;set;}
    
    private String [] allTableCreation = {
               @"CREATE TABLE IF NOT EXISTS [CyaBucket]
                (
                    [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    [MainTokenId] INTEGER NOT NULL,
                    [Data] NVARCHAR(40000) check(length(Data) <= 40000),
                    [Created] NVARCHAR(30) default (datetime('now','localtime')) check(length(Created) <= 30),
                    [Updated] NVARCHAR(30) check(length(Updated) <= 30),
                    [Active] BOOLEAN default(1)
                )"};

    public SqliteCyaProvider()
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
                    Console.WriteLine($"key: {key}");
                }
            }
            return allTokens;
        }
        catch(Exception ex){
            Console.WriteLine($"Error: {ex.Message}");
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
        catch(Exception ex){
            Console.WriteLine($"Error: {ex.Message}");
            return 0;
        }
        finally{
            if (connection != null){
                connection.Close();
            }
        }
    }

    public Bucket GetBucket(){
        try{
            Console.WriteLine("GetCyaBucket...");
            connection.Open();
            Console.WriteLine("Opening...");
            using (var reader = command.ExecuteReader())
            {
                reader.Read();
                var id = reader.GetInt64(0);
                var mainTokenId = reader.GetInt64(1);
                var data = reader.GetString(2);
                var created = "";
                if (!reader.IsDBNull(3)){
                    created = reader.GetString(3);
                }
                var updated = "";
                if (!reader.IsDBNull(4)){
                    updated = reader.GetString(4);
                }
                var active = reader.GetBoolean(5);
                Bucket b = new Bucket(id,mainTokenId,data,created,updated,active);
                Console.WriteLine($"GetBucket() id: {b.Id}");
                reader.Close();
                return b;
            }
        }
        catch(Exception ex){
            Console.WriteLine($"Error: {ex.Message}");
            return new Bucket(0,0);
        }
        finally{
            if (connection != null){
                connection.Close();
            }
        }
    }
    
    public Int64 Save(){
        
        try{
            Console.WriteLine("Saving...");
            connection.Open();
            Console.WriteLine("Opened.");
            // id should be last id inserted into table
            var id = Convert.ToInt64(command.ExecuteScalar());
            Console.WriteLine("inserted.");
            return id;
        }
        catch(Exception ex){
            Console.WriteLine($"Error: {ex.Message}");
            return 0;
        }
        finally{
            if (connection != null){
                connection.Close();
            }
        }
    }
}