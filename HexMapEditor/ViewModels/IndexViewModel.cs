using Microsoft.AspNetCore.Html;

namespace HexMapEditor.Data;
public class IndexViewModel
{
    public HtmlString TileMapString { get; set; }
	public HtmlString AssetList{get;set;}
}