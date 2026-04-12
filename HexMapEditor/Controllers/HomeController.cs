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

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("/{user}"), Route("/")]
    public IActionResult Index(string user)
    {
        bool isAdmin = false;
        int duty = 0;
        int leisure = 0;
        if (string.IsNullOrEmpty(user))
        {
            user = "guest";
        } else
        {
            user = user.ToLower();
            JsonNode userInfo  = Users.GetUsers()[user];
            if (userInfo != null)
            {
                isAdmin = (bool)userInfo["admin"];
                duty = (int)userInfo["duty"];
                leisure = (int)userInfo["leisure"];
            }
        }
        // WriteDefaultValues();
        Tilemap tilemap = PullTilemap();
        HtmlString tilemapString = new(tilemap.ToJson());

        // How to deserialize json, goddamn:
		// List<List<List<string>>> json = JsonSerializer.Deserialize<List<List<List<string>>>>((string)tilemapString);
        Console.WriteLine(tilemapString);
        IndexViewModel viewModel = new IndexViewModel
        {
            TileMapString = tilemapString,
            User = user,
            IsAdmin = isAdmin,
            Duty = duty,
            Leisure = leisure
        };
		return View(viewModel);
    }

    [HttpPost]
    [Route("/{user}"), Route("/")]
    public IActionResult Index(string user, string transferForm)
    {
        Console.Write("Post submitted: ");
        Console.WriteLine(transferForm);

        Tilemap tilemap = new(transferForm);
        tilemap.SaveToFile();
        return RedirectToAction("Index");
    }

    public IActionResult Campsite()
    {
        return View();
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

    public Tilemap PullTilemap()
    {
        Tilemap tilemap = new();
        tilemap.PullFromFile();
        tilemap.grid_height = 10;
        tilemap.grid_width = 10;
        return tilemap;
    }

    public static void WriteDefaultValues()
    {
        Tilemap tilemap = new();
        tilemap.SetCell(0, 0, ["grass"]);
        tilemap.SetCell(1, 0, ["grass"]);
        tilemap.SetCell(0, 1, ["grass"]);
        tilemap.grid_height = 10;
        tilemap.grid_width = 10;
        tilemap.SaveToFile();
        ;
    }
}
