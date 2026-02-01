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

class Tilemap()
{
	Dictionary<int, Dictionary<int, Cell>> MapH{get; set;} = [];
	public void AddCellLayer(int x, int y, string layer)
	{
		MapH[x][y].contents.Add(layer);
		return;
	}

	public List<string> GetCell(int x, int y)
	{
		return MapH[x][y].contents;
	}

	public bool SaveToBool()
	{
		return false;
	}
}