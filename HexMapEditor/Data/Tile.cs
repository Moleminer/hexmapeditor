using Microsoft.AspNetCore.Html;

namespace HexMapEditor.Data;
public class Tile
{
    public List<string> Values { get; set; }
	public int X {get;set;}
    public int Y { get; set; }
	public string Description { get; set; }

}