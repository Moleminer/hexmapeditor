using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HexMapEditor.Models;
using HexMapEditor.Data;
using System;
using System.IO;
using System.Text;

namespace HexMapEditor.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        
        Tilemap tilemap = PullTilemap();
        return View(tilemap.ToList());
    }

    public IActionResult Privacy()
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
        String line;
        try
        {
            //Pass the file path and file name to the StreamReader constructor
            StreamReader sr = new StreamReader(@"Content\tilemap.txt");
            //Read the first line of text
            line = sr.ReadLine();
            int curr_x = 0;
            int curr_y = 0;
            //Continue to read until you reach end of file
            
            while (line != null)
            {
                //write the line to console window
                // Console.WriteLine(line);
                line = line.TrimStart();

                Cell cell = new()
                    {
                        x = curr_x,
                        y = curr_y,
                        contents = []
                    };
                Boolean breakout = false;
                foreach (char c in line)
                {
                    if (breakout)
                    {
                        break;
                    }
                    switch (c)
                    {
                        case ' ':
                            if (cell.contents.Count > 0)
                            {
                                tilemap.OverwriteCell(cell.x, cell.y, cell);
                            }
                            curr_x += 2;
                            cell = new()
                            {
                                x = curr_x,
                                y = curr_y,
                                contents = []
                            };
                            break;
                        case 'g':
                            cell.contents.Add("grass");
                            break;
                        case 'w':
                            cell.contents.Add("water");
                            break;
                        case 'd':
                            cell.contents.Add("desert");
                            break;
                        case 'r':
                            cell.contents.Add("rocky");
                            break;
                        case 's':
                            cell.contents.Add("swamp");
                            break;
                        case '?':
                            cell.contents.Add("fogowar");
                            break;
                        case 'M':
                            cell.contents.Add("mountains");
                            break;
                        case 'H':
                            cell.contents.Add("hills");
                            break;
                        case 'T':
                            cell.contents.Add("trees");
                            break;
                        case 'B':
                            cell.contents.Add("buildings");
                            break;
                        case '/':
                        case '#':
                            curr_x = (curr_x+1) % 2;
                            curr_y -= 1;
                            breakout = true;
                            break;
                        default:
                            Console.WriteLine("Unrecognised instruction ");
                            break;

                    }
                }
                //Read the next line
                line = sr.ReadLine();
                curr_x = (curr_x+1) % 2;
                curr_y += 1;
            }
            //close the file
            sr.Close();
        }
        catch(Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
            Console.WriteLine(e.StackTrace);
        }
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
