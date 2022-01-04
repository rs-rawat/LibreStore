namespace LibreStore.Models;

public class Usage{
    public int Id{get;set;}
    
    public int MainTokenId{get;set;}
    public String IpAddress{get;set;}
    
    public String Action{get;set;}
    public DateTime Created {get;set;}
    public bool Active{get;set;}

    public Usage(int mainTokenId, String ipAddress, String action="")
    {
        MainTokenId = mainTokenId;
        IpAddress = ipAddress;
        Action = action;
        Created = DateTime.Now;
        Active = true;
    }
}