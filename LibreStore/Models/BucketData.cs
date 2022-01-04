namespace LibreStore.Models;
public class BucketData{

    private IPersistable dataPersistor;

    private Bucket bucket;
    public BucketData(IPersistable dataPersistor, Bucket bucket)
    {
        this.dataPersistor = dataPersistor;
        this.bucket = bucket;
        
    }

    public int Configure(){
        if (dataPersistor != null)
        {
            SqliteProvider sqliteProvider = dataPersistor as SqliteProvider;
            
            sqliteProvider.command.CommandText = @"INSERT into Bucket (mainTokenId,data)values($mainTokenId,$data)";
            sqliteProvider.command.Parameters.AddWithValue("$mainTokenId",bucket.MainTokenId);
            sqliteProvider.command.Parameters.AddWithValue("$data",bucket.Data);
            return 0;
        }
        return 1;
    }
}