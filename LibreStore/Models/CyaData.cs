namespace LibreStore.Models;
public class CyaData{

    private IPersistable dataPersistor;

    private Cya cya;
    public CyaData(IPersistable dataPersistor, Cya cya)
    {
        this.dataPersistor = dataPersistor;
        this.cya = cya;
    }

    public int Configure(){
        if (dataPersistor != null)
        {
            SqliteProvider sqliteProvider = dataPersistor as SqliteProvider;
            
            sqliteProvider.command.CommandText = @"INSERT into Cya (mainTokenId,data)values($mainTokenId,$data);SELECT last_insert_rowid()";
            sqliteProvider.command.Parameters.AddWithValue("$mainTokenId",cya.MainTokenId);
            sqliteProvider.command.Parameters.AddWithValue("$data",cya.Data);
            return 0;
        }
        return 1;
    }

    public int ConfigureSelect(String key){
        if (dataPersistor != null)
        {
            SqliteProvider sqliteProvider = dataPersistor as SqliteProvider;
            sqliteProvider.command.CommandText = @"select b.* from MainToken as mt 
                    join cya as b on mt.id = b.mainTokenId 
                    where mt.Key=$key and b.Id = $id
                    and b.active = 1 and mt.active=1";
            sqliteProvider.command.Parameters.AddWithValue("$key",key);
            sqliteProvider.command.Parameters.AddWithValue("$id",cya.Id);
            return 0;
        }
        return 1;
    }
}