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



}