namespace LibreStore.Models;

public class Usage{
    public int Id{get;set;}
    public String IpAddress{get;set;}

    public String Action{get;set;}
    public DateTime Created {get;set;}
    public bool Active{get;set;}

    public Usage(String ipAddress, String action="")
    {
        IpAddress = ipAddress;
        Action = action;
        Created = DateTime.Now;
        Active = true;
    }
}