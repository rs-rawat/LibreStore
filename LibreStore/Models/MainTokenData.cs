namespace LibreStore.Models;
public class MainTokenData{

    private IPersistable dataPersistor;

    MainToken MainToken;
    public MainTokenData(IPersistable dataPersistor, MainToken mainToken)
    {
        this.dataPersistor = dataPersistor;
        this.MainToken = mainToken;
        
    // @"
    //     SELECT name
    //     FROM user
    //     WHERE id = $id
    // ";
    // command.Parameters.AddWithValue("$id", id);
    }

    public void ConfigInsert(){
        
    }
}