using Microsoft.AspNetCore.Mvc;
using LibreStore.Models;
using System.Security.Cryptography;
using System.Text;

namespace LibreStore.Controllers;

[ApiController]
[Route("[controller]")]
public class DataController : Controller
{
    private readonly ILogger<DataController> _logger;

    public DataController(ILogger<DataController> logger)
    {
        _logger = logger;
    }

    [HttpGet("SaveToken")]
    public ActionResult SaveToken(String key){

        MainToken mt = new MainToken(key);
        
        SqliteProvider sp = new SqliteProvider();
        WriteUsage(sp,"SaveToken",key);

        // Had to new up the SqliteProvider to insure it was initialized properly
        // for use with MainTokenData
        sp = new SqliteProvider();
        MainTokenData mtd = new MainTokenData(sp,mt);
        mtd.Configure();
        sp.Save();
        
        var jsonResult = new {success=true};
        return new JsonResult(jsonResult);
    }

    [HttpGet("SaveData")]
    public ActionResult SaveData(String key, String data){
        SqliteProvider sp = new SqliteProvider();
        var mainTokenId = WriteUsage(sp,"SaveData",key);
        // if mainTokenId == 0 then an error occurred.
        if (mainTokenId == 0){
            var jsonErrorResult = new {success=false,message="Couldn't save data because of invalid MainToken.Key."};
            return new JsonResult(jsonErrorResult);    
        }
        sp = new SqliteProvider();
        Bucket b = new Bucket(mainTokenId,data);
        BucketData bd = new BucketData(sp,b);
        bd.Configure();
        sp.Save();
    
        var jsonResult = new {success=true};
        return new JsonResult(jsonResult);
    }

    [HttpGet("GetAllTokens")]
    public ActionResult GetAllTokens(String pwd){
        List<MainToken> allTokens = new List<MainToken>();
        SqliteProvider sp = new SqliteProvider();
        if (Hash(pwd) != "86BC2CA50432385C30E2FAC2923AA6D19F7304E213DAB1D967A8D063BEF50EE1"){
            WriteUsage(sp,"GetAllTokens - rejected");
            return new JsonResult(new {result="false",message="couldn't authenticate request"});
        }
        sp = new SqliteProvider();
        allTokens = sp.GetAllTokens();

        sp = new SqliteProvider();
        // just want to get IP Address of 
        WriteUsage(sp,"GetAllTokens");
        
        return new JsonResult(allTokens);
    }

    private int WriteUsage(SqliteProvider sp, String action, String key=""){
        var ipAddress = Request?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "0.0.0.0";
        MainTokenData mtd = new MainTokenData(sp,new MainToken(key));
        mtd.ConfigureInsert();
        var mainTokenId = sp.GetOrInsert();
        Usage u = new Usage(mainTokenId,ipAddress,action);
        UsageData ud = new UsageData(sp,u);
        ud.Configure();
        sp.Save();
        return mainTokenId;
    }

    public string Hash(string value) 
    { 
        var sha = SHA256.Create();
        byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(value)); 
        return String.Concat(Array.ConvertAll(hash, x => x.ToString("X2"))); 
    }

    // public IActionResult Index()
    // {
    //     return View();
    // }
   
}
