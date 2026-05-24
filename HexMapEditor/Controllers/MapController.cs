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
using System.Security.Claims;

namespace HexMapEditor.Controllers;

public class MapController : Controller
{
    private readonly ILogger<MapController> _logger;

    public MapController(ILogger<MapController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {

        // WriteDefaultValues();
        Tilemap tilemap = PullTilemap();
        HtmlString tilemapString = new(tilemap.ToJson());

        // Now grab assets
        AssetList assetList = new();
        assetList.PullFromFile();
        HtmlString assetListString = new(assetList.ToJson());

        // How to deserialize json, goddamn:
		// List<List<List<string>>> json = JsonSerializer.Deserialize<List<List<List<string>>>>((string)tilemapString);
        // Console.WriteLine(tilemapString);
        IndexViewModel viewModel = new IndexViewModel
        {
            TileMapString = tilemapString,
            AssetList = assetListString,
        };
		return View(viewModel);
    }


	// [HttpPost]
    // [Route("/{user}"), Route("/")]
    // public IActionResult Index(string user, string transferForm)
    // {
    //     Console.Write("Post submitted: ");
    //     Console.WriteLine(transferForm);

    //     Tilemap tilemap = new(transferForm);
    //     tilemap.SaveToFile();
    //     return RedirectToAction("Index");
    // }
	[HttpPost]
	public IActionResult UpdateNote(string noteinput, int x, int y)
	{
		//TODO: change the above to a record to add serverside validation
		Tilemap tilemap = new();
		tilemap.PullFromFile();
		Tile cell = tilemap.GetCell(x, y);
		cell.Description = noteinput;
		tilemap.SetCell(x, y, cell);
		tilemap.SaveToFile();
		return RedirectToAction(nameof(Index));
	}

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

	public IActionResult RawJson()
	{
		Tilemap tilemap = PullTilemap();
        HtmlString tilemapString = new(tilemap.ToJson());
		return View(tilemapString);
	}

    public Tilemap PullTilemap()
    {
        Tilemap tilemap = new();
        tilemap.PullFromFile();
        return tilemap;
    }

    public static void WriteDefaultValues()
    {
        Tilemap tilemap = new();
        tilemap.SetCell(0, 0, new Tile{
			X = 0,
			Y = 0,
			Values = ["Grass"]	
		});
		tilemap.SetCell(1, 0, new Tile{
			X = 1,
			Y = 0,
			Values = ["Grass"]	
		});
		tilemap.SetCell(0, 1, new Tile{
			X = 0,
			Y = 1,
			Values = ["Grass"]	
		});
        tilemap.SaveToFile();
        ;
    }
}
