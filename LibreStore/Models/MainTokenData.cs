namespace LibreStore.Models;
public class MainTokenData{

    private IPersistable dataPersistor;

    private MainToken mainToken;
    public MainTokenData(IPersistable dataPersistor, MainToken mainToken)
    {
        this.dataPersistor = dataPersistor;
        this.mainToken = mainToken;
        
    }

    public int Configure(){
        if (dataPersistor != null)
        {
            SqliteProvider sqliteProvider = dataPersistor as SqliteProvider;
            
            sqliteProvider.command.CommandText = @"INSERT into MainToken (key)values($key)";
            sqliteProvider.command.Parameters.AddWithValue("$key",mainToken.Key);
            return 0;
        }
        return 1;
    }

    public int ConfigureInsert(){
        if (dataPersistor != null)
        {
            SqliteProvider sqliteProvider = dataPersistor as SqliteProvider;
            String sqlCommand = @"insert into maintoken (key)  
                    select $key 
                    where not exists 
                    (select key from maintoken where key=$key);
                     select id from maintoken where key=$key and active=1";
            
            sqliteProvider.command.CommandText = sqlCommand;
            sqliteProvider.command.Parameters.AddWithValue("$key",mainToken.Key);
            return 0;
            
        }
        return 2;

    }



}