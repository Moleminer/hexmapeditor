using Microsoft.AspNetCore.Html;

namespace HexMapEditor.Data;
public class IndexViewModel
{
    public HtmlString TileMapString { get; set; }
	public HtmlString AssetList{get;set;}
    public string User { get; set; }
	public bool IsAdmin {get;set;}
	public int Duty {get;set;}
	public int Leisure {get;set;}
}