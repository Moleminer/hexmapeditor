using Microsoft.AspNetCore.Html;

namespace HexMapEditor.Data;
public class IndexViewModel
{
    public HtmlString TileMapString { get; set; }
    public string User { get; set; }
}