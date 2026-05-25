using Microsoft.AspNetCore.Html;

namespace HexMapEditor.Data;
public class RenderMapViewModel
{
    public HtmlString TileMapString { get; set; }
	public HtmlString AssetList{get;set;}
}