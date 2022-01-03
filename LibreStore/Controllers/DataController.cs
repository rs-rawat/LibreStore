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
        
        
        MainTokenData mtd = new MainTokenData(sp,mt);
        mtd.Configure();
        sp.Save();
        
        var jsonResult = new {success=true};
        return new JsonResult(jsonResult);
    }

    [HttpGet("SaveData")]
    public ActionResult SaveData(String key, String data){
        var jsonResult = new {success=true};
        return new JsonResult(jsonResult);
    }

    // public IActionResult Index()
    // {
    //     return View();
    // }
   
}
