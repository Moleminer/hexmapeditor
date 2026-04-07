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
        // Tilemap tilemap = new();
        // String line;
        // try
        // {
        //     //Pass the file path and file name to the StreamReader constructor
        //     StreamReader sr = new StreamReader(@"Content\tilemap.txt");
        //     //Read the first line of text
        //     line = sr.ReadLine();
        //     //Continue to read until you reach end of file
            
        //     while (line != null)
        //     {
        //         //write the line to console window
        //         // Console.WriteLine(line);
        //         line = line.TrimStart();

        //         List<List<string>> grid_line = [[]];
		// 		bool breakout = false;
        //         foreach (char c in line)
        //         {
        //             if (breakout)
        //             {
        //                 break;
        //             }
        //             switch (c)
        //             {
        //                 case ' ':
        //                     if (grid_line.Count < 1 || grid_line.Last().Count > 0)
        //                     {
        //                         grid_line.Add([]);
        //                     }

        //                     break;
        //                 case 'g':
        //                     grid_line.Last().Add("grass");
        //                     break;
        //                 case 'w':
        //                     grid_line.Last().Add("water");
        //                     break;
        //                 case 'd':
        //                     grid_line.Last().Add("desert");
        //                     break;
        //                 case 'r':
        //                     grid_line.Last().Add("rocky");
        //                     break;
        //                 case 's':
        //                     grid_line.Last().Add("swamp");
        //                     break;
        //                 case '?':
        //                     grid_line.Last().Add("fogowar");
        //                     break;
        //                 case 'M':
        //                     grid_line.Last().Add("mountains");
        //                     break;
        //                 case 'H':
        //                     grid_line.Last().Add("hills");
        //                     break;
        //                 case 'T':
        //                     grid_line.Last().Add("trees");
        //                     break;
        //                 case 'B':
        //                     grid_line.Last().Add("buildings");
        //                     break;
        //                 case '/':
        //                 case '#':
        //                     breakout = true;
        //                     break;
        //                 default:
        //                     Console.Error.WriteLine("Unrecognised instruction ");
        //                     break;

        //             }
        //         }
        //         // Line to cut out empty cell at end
        //         if (grid_line.Count > 0 && grid_line.Last().Count < 1)
        //         {
        //             grid_line.RemoveAt(grid_line.Count - 1);
        //         }
        //         tilemap.AppendXLayer(grid_line);
        //         //Read the next line
        //         line = sr.ReadLine();
        //     }
        //     //close the file
        //     sr.Close();
        // }
        // catch(Exception e)
        // {
        //     Console.WriteLine("Exception: " + e.Message);
        //     Console.WriteLine(e.StackTrace);
        // }
        Tilemap tilemap = new();
        tilemap.SetCell(0, 0, ["grass"]);
        tilemap.SetCell(1, 0, ["grass"]);
        tilemap.SetCell(0, 1, ["grass"]);
        tilemap.grid_height = 10;
        tilemap.grid_width = 10;
        return tilemap;
    }
    public static void WriteDefaultValues()
    {
        // Tilemap tilemap = new Tilemap();
        // Cell c = new Cell
        // {
        //     x = 1,
        //     y = 2,
        //     contents = new List<String>(["empty"])
        // };

        // using (var stream = File.Open("empty.bin", FileMode.Create))
        // {
		// 	using var writer = new BinaryWriter(stream, Encoding.UTF8, false);

		// 	writer.Write(c.to);
		// }
        ;
    }
}
