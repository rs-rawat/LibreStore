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

    [HttpGet("DoWork")]
    public ActionResult DoWork(String key){

        MainToken mt = new MainToken(key);
        PropertyInfo[] properties = mt.GetType().GetProperties();
        foreach (var p in properties)
        {   
            var myVal = $"{p.Name} : {p.PropertyType} :{p.GetValue(mt)}";
            Console.WriteLine(myVal);
        }
        String mainTokenInsert = @"INSERT into MainToken (key)values($key)";
        SqliteProvider sp = new SqliteProvider(mainTokenInsert);
        sp.ConfigInsert(mt);
        MainTokenData mtd = new MainTokenData(sp,mt);
        sp.Save();
        
        var jsonResult = new {success=true};
        return new JsonResult(jsonResult);
    }

    // public IActionResult Index()
    // {
    //     return View();
    // }
   
}
