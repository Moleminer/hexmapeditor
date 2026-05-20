using Microsoft.AspNetCore.Html;

namespace HexMapEditor.Data;
public class ProfileViewModel
{
    public string User { get; set; }
	public bool IsAdmin {get;set;}
	public int Duty {get;set;}
	public int Leisure {get;set;}
}