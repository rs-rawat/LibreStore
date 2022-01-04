using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibreStore.Models;
using System.Reflection;

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
        
        WriteUsage(sp,"SaveToken");

        MainTokenData mtd = new MainTokenData(sp,mt);
        mtd.Configure();
        sp.Save();
        
        var jsonResult = new {success=true};
        return new JsonResult(jsonResult);
    }

    [HttpGet("SaveData")]
    public ActionResult SaveData(String key, String data){
        SqliteProvider sp = new SqliteProvider();
        WriteUsage(sp,"SaveData");
        
        var jsonResult = new {success=true};
        return new JsonResult(jsonResult);
    }

    private void WriteUsage(SqliteProvider sp, String action){
        var ipAddress = Request.HttpContext.Connection.RemoteIpAddress;
        
        Usage u = new Usage(ipAddress.ToString(),action);
        UsageData ud = new UsageData(sp,u);
        ud.Configure();
        sp.Save();
    }

    // public IActionResult Index()
    // {
    //     return View();
    // }
   
}
