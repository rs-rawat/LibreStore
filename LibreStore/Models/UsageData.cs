namespace LibreStore.Models;
public class UsageData{

    private IPersistable dataPersistor;

    private Usage usage;
    public UsageData(IPersistable dataPersistor, Usage usage)
    {
        this.dataPersistor = dataPersistor;
        this.usage = usage;
        
    }

    public int Configure(){
        if (dataPersistor != null)
        {
            SqliteProvider sqliteProvider = dataPersistor as SqliteProvider;
            
            sqliteProvider.command.CommandText = @"INSERT into Usage (ipaddress,action)values($ipaddress,$action)";
            sqliteProvider.command.Parameters.AddWithValue("$ipaddress",usage.IpAddress);
            sqliteProvider.command.Parameters.AddWithValue("$action", usage.Action);
            return 0;
        }
        return 1;
    }



}