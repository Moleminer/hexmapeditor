using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HexMapEditor.Models;
using HexMapEditor.Data;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Web;
using Microsoft.AspNetCore.Html;
using System.Text.Json.Nodes;

namespace HexMapEditor.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private RGRContext _context ;

    public HomeController(ILogger<HomeController> logger, RGRContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return RedirectToAction("Index", "Map");
    }
    public IActionResult Campsite()
    {
        return RedirectToAction("Index", "Town");
    }

     public IActionResult About()
    {
        
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
