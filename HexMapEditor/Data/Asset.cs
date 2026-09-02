using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Html;

namespace HexMapEditor.Data;
public class Asset
{
    public string Name { get; set; }
    [Key]
	public string Filename {get;set;}
    public double Scale { get; set; }

}
