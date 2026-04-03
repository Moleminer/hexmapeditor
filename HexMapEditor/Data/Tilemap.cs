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

	public void AppendXLayer(List<List<string>> input)
	{
		grid.Add(input);
	}

	public void AppendYLayer(List<List<string>> input)
	{
		int i = 0;
		foreach (List<string> s in input)
		{
			grid.ElementAt(i).Add(s);
			i++;
		}
	}
	public void AddCellLayer(int x, int y, string layer)
	{
		try
		{
			grid.ElementAt(x).ElementAt(y).Append(layer);	
		} catch (ArgumentOutOfRangeException)
		{
			Console.Error.WriteLine("Tried to add a layer to a cell that didn't exist.");
		}
	}

	public void OverwriteCell(int x, int y, List<string> cell)
	{
		grid.ElementAt(x).ElementAt(y).Clear();
		grid.ElementAt(x).ElementAt(y).AddRange(cell);
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