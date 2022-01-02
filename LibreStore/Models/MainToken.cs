namespace LibreStore.Models;

public class MainToken{
    public int Id{get;set;}
    public int OwnerId{get;set;}
    public String Key{get; set;}
    public DateTime Created {get;set;}
    public bool Active{get;set;}

    public MainToken(String key, int ownerId=0)
    {
        OwnerId = ownerId;
        Key = key;
        Created = DateTime.Now;
        Active = true;
    }
}