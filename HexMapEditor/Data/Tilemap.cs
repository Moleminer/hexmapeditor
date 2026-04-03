using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Json;

namespace HexMapEditor.Data;


public class Tilemap
{

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


	// public List<List<List<string>>> ToList()
	// {
	// 	return false;
	// }

	public string ToJson()
	{
		return System.Text.Json.JsonSerializer.Serialize(grid);
	}

	// public void AdjustRange(int x, int y)
	// {
	// 	MinX ??= x;
	// 	MinY ??= y;
	// 	MaxX ??= x;
	// 	MaxY ??= y;
	// 	if (x < MinX) MinX = x;
	// 	if (y < MinY) MinY = y;
	// 	if (x < MaxX) MaxX = x;
	// 	if (y < MaxY) MaxY = y;
	// }
}