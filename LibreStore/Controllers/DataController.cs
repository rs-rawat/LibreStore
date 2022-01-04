using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibreStore.Models;
using System.Reflection;
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
        PropertyInfo[] properties = mt.GetType().GetProperties();
        foreach (var p in properties)
        {   
            var myVal = $"{p.Name} : {p.PropertyType} :{p.GetValue(mt)}";
            Console.WriteLine(myVal);
        }
        
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
        WriteUsage(sp,"SaveData",key);
        
        var jsonResult = new {success=true};
        return new JsonResult(jsonResult);
    }

    [HttpGet("GetAllTokens")]
    public ActionResult GetAllTokens(String pwd){
        SqliteProvider sp = new SqliteProvider();
        List<MainToken> allTokens = sp.GetAllTokens();
        if (Hash(pwd) != "86BC2CA50432385C30E2FAC2923AA6D19F7304E213DAB1D967A8D063BEF50EE1"){
            return new JsonResult(new {result="false",message="couldn't authenticate request"});
        }

        return new JsonResult(allTokens);
    }

    private void WriteUsage(SqliteProvider sp, String action, String key){
        var ipAddress = Request.HttpContext.Connection.RemoteIpAddress;
        MainTokenData mtd = new MainTokenData(sp,new MainToken(key));
        mtd.ConfigureInsert();
        var mainTokenId = sp.GetOrInsert();
        Usage u = new Usage(mainTokenId,ipAddress.ToString(),action);
        UsageData ud = new UsageData(sp,u);
        ud.Configure();
        sp.Save();
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
