using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Html;

namespace HexMapEditor.Data;
public class LoginViewModel
{
	[Required]
    [Display(Name = "Enter Username:")]
	public string Username {get;set;}
}