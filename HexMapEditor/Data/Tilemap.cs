using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http.Json;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http.Features;
using System.Text.Json;
using System;
using System.IO;
using System.Text;

namespace HexMapEditor.Data;


public class Tilemap
{
	private readonly string path = @"Content\tilemap.json";
	public int grid_height {get; set;}
	public int grid_width {get; set;}
	Dictionary<ValueTuple<int, int>, Tile> grid = [];
	
	public Tilemap() {;}
	public Tilemap(string jsonInput)
	{
		ParseJson(jsonInput);
		return;
	}

	public Tilemap(int x, int y)
	{
		for (int i = 0; i < x; i++)
		{
			for (int j = 0; j < y; j++)
			{
				grid.Add((i, j), new Tile
				{
					X = i,
					Y = j,
					Values = [],
					Description = ""
				});
			}
		}
	}

	public void AddCellLayer(int x, int y, string layer)
	{
		try
		{
			grid[(x,y)].Values.Add(layer);
			grid_height = Math.Max(grid_height, y);
			grid_width = Math.Max(grid_width, x);
		} catch (KeyNotFoundException)
		{
			grid.Add((x, y), new Tile {
				X = x,
				Y = y,
				Values = [layer],
				Description = ""
				});
		} catch (Exception)
		{
			Console.Error.WriteLine("Something went wrong adding layer to cell");
		}
	}

	public void SetCell(int x, int y, List<string> cell)
	{
		try
		{
			grid[(x,y)].Values = cell;
			grid_height = Math.Max(grid_height, y);
			grid_width = Math.Max(grid_width, x);
		} catch (KeyNotFoundException)
		{
			grid.Add((x, y), new Tile {
				X = x,
				Y = y,
				Values = cell,
				Description = ""
				});
		} catch (Exception)
		{
			Console.Error.WriteLine("Something went wrong writing to cell");
		}
		
	}

	public Tile GetCell(int x, int y)
	{
		try
		{
			return grid[(x, y)];
		} catch
		{
			Console.Error.WriteLine("Failed to find cell.");
			return new Tile();
		}
	}

	public bool SaveToFile()
	{
        string jsonString = ToJson();
		File.WriteAllText(path, jsonString);
		return true;
	}

	public void PullFromFile()
	{
		List<string> fileLines = File.ReadLines(path).ToList<string>();
		ParseJson(fileLines.ElementAt(0));
	}


	public string ToJson()
	{

		List<Tile> flatGrid = grid.Values.ToList();
		return JsonSerializer.Serialize(flatGrid);
	}

	private static void AddText(FileStream fs, string value)
    {
        byte[] info = new UTF8Encoding(true).GetBytes(value);
        fs.Write(info, 0, info.Length);
    }

	private void ParseJson(string input)
	{
		JsonArray node = JsonNode.Parse(input).AsArray();

		foreach (dynamic item in node)
		{
			// Console.WriteLine(item["Values"].AsArray()[0]);
			List<string> values = [];
			foreach(string str in item["Values"].AsArray())
			{
				values.Add(str);
			}
			int x = (int)item["X"];
			int y = (int)item["Y"];
			grid.Add((x,y), new Tile {
				Values = values,
				X = x,
				Y = y,
				Description = (string)item["Description"]
			}
			);
		}
	}

}