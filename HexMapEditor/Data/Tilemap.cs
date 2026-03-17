using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace HexMapEditor.Data;

public struct Cell
	{
		public int x;
        public int y;
		public List<string> contents;
	}

public class Tilemap()
{
	Dictionary<Tuple<int, int>, Cell> MapH{get; set;} = [];
	int? MaxX = null;
	int? MaxY = null;
	int? MinX = null;
	int? MinY = null;
	public void AddCellLayer(int x, int y, string layer)
	{
		var coords = Tuple.Create(x, y);
		if (MapH.TryGetValue(coords, out Cell value))
		{
			value.contents.Add(layer);
		} else
		{
			MapH[coords] = new Cell
			{
				x = x,
				y = y,
				contents = [layer]
			};
			AdjustRange(x, y);
		}
		return;
	}

	public void OverwriteCell(int x, int y, Cell cell)
	{
		Tuple<int,int> coords = new(x, y);
		MapH[coords] = cell;
		AdjustRange(x, y);
	}

	public List<string> GetCell(int x, int y)
	{
		Tuple<int, int> ghgh = new(x,y);
		if (MapH.ContainsKey(ghgh)) {
			return MapH[ghgh].contents;
		} else
		{
			Console.WriteLine("FAILURE TO RETRIEVE CELL: " + x + y);
			return [];
		}
	}

	public bool SaveToBool()
	{
		return false;
	}


	public List<object> ToList()
	{
		Console.WriteLine("Writing to list");
		List<object> returnList = [new List<int?>([MinX, MinY]), new List<int?>([MaxX, MaxY])];
		foreach (Tuple<int,int> i in MapH.Keys)
		{
			// Console.WriteLine(i.Item1 + " " + i.Item2 + " " + MapH[i].contents.Count);
			List<object> sublist = [i.Item1, i.Item2, new List<string>(MapH[i].contents)];
			returnList.Add(sublist);
		}
			
		
		return returnList;
	}

	public List<object> ToJson()
	{
		Console.WriteLine("Writing to list");
		List<object> returnList = [new List<int?>([MinX, MinY]), new List<int?>([MaxX, MaxY])];
		foreach (Tuple<int,int> i in MapH.Keys)
		{
			// Console.WriteLine(i.Item1 + " " + i.Item2 + " " + MapH[i].contents.Count);
			List<object> sublist = [i.Item1, i.Item2, new List<string>(MapH[i].contents)];
			returnList.Add(sublist);
		}
			
		
		return returnList;
	}

	public void AdjustRange(int x, int y)
	{
		MinX ??= x;
		MinY ??= y;
		MaxX ??= x;
		MaxY ??= y;
		if (x < MinX) MinX = x;
		if (y < MinY) MinY = y;
		if (x < MaxX) MaxX = x;
		if (y < MaxY) MaxY = y;
	}
}