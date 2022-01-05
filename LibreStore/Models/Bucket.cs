namespace LibreStore.Models;

public class Bucket{
    public Int64 Id{get;set;}
    public Int64 MainTokenId{get;set;}
    public String? Data{get;set;}
    public DateTime? Created {get;set;}
    public DateTime? Updated{get;set;}
    public bool Active{get;set;}

    public Bucket(Int64 id, Int64 mainTokenId){
        Id = id;
        MainTokenId = mainTokenId;
    }

    public Bucket(Int64 mainTokenId, String data)
    {
        MainTokenId = mainTokenId;
        Data = data;
    }

    public Bucket(Int64 id, Int64 mainTokenId, 
        String data, String created, string updated, bool active)
    {
        Id = id;
        MainTokenId = mainTokenId;
        Data = data;
        Created = created != String.Empty ? DateTime.Parse(created) : null;
        Updated = updated != String.Empty ? DateTime.Parse(updated) : null;
        Active = active;
    }
    
}