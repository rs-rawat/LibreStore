namespace LibreStore.Models;

public class Bucket{
    public int Id{get;set;}
    public int MainTokenId{get;set;}
    public String Data{get;set;}
    public DateTime Created {get;set;}
    public DateTime Updated{get;set;}
    public bool Active{get;set;}
    public Bucket(int mainTokenId, String data)
    {
        MainTokenId = mainTokenId;
        Data = data;
    }
}