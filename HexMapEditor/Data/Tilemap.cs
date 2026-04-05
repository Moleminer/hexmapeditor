using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace HexMapEditor.Data;


public class Tilemap
{
	public int grid_height {get; set;}
	public int grid_width {get; set;}
	Dictionary<ValueTuple<int, int>, List<string>> grid = [];
	
	public Tilemap() {;}

	public Tilemap(int x, int y)
	{
		for (int i = 0; i < x; i++)
		{
			for (int j = 0; j < y; j++)
			{
				grid.Add((1, 2), []);
			}
		}
	}

	public void AddCellLayer(int x, int y, string layer)
	{
		try
		{
			grid[(x,y)].Add(layer);
			grid_height = Math.Max(grid_height, y);
			grid_width = Math.Max(grid_width, x);
		} catch (KeyNotFoundException)
		{
			grid.Add((x, y), [layer]);
		} catch (Exception)
		{
			Console.Error.WriteLine("Something went wrong adding layer to cell");
		}
	}

	public void SetCell(int x, int y, List<string> cell)
	{
		try
		{
			grid[(x,y)] = cell;
			grid_height = Math.Max(grid_height, y);
			grid_width = Math.Max(grid_width, x);
		} catch (KeyNotFoundException)
		{
			grid.Add((x, y), cell);
		} catch (Exception)
		{
			Console.Error.WriteLine("Something went wrong writing to cell");
		}
		
	}

	public List<string> GetCell(int x, int y)
	{
		try
		{
			return grid[(x, y)];
		} catch
		{
			Console.Error.WriteLine("Failed to find cell.");
			return [];
		}
	}

	public bool SaveToBool()
	{
		return false;
	}


	public string ToJson()
	{

		List<GridCellDto> flatGrid = grid.Select(item => new GridCellDto 
		{ 
			X = item.Key.Item1, 
			Y = item.Key.Item2, 
			Values = item.Value 
		}).ToList();
		return JsonSerializer.Serialize(flatGrid);
	}

}