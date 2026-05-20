using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HexMapEditor.Models;
using HexMapEditor.Data;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Web;
using Microsoft.AspNetCore.Html;
using System.Text.Json.Nodes;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace HexMapEditor.Controllers;

public class ProfileController(ILogger<ProfileController> logger) : Controller
{
    private readonly ILogger<ProfileController> _logger = logger;

	[HttpGet]
    public IActionResult EditMap(string id)
    {
		string user = id;
        bool isAdmin = false;
        int duty = 0;
        int leisure = 0;
        if (string.IsNullOrEmpty(user))
        {
            user = "guest";
        } else
        {
            user = user.ToLower();
            JsonNode userInfo  = Users.GetUsers()[user];
            if (userInfo != null)
            {
                isAdmin = (bool)userInfo["admin"];
                duty = (int)userInfo["duty"];
                leisure = (int)userInfo["leisure"];
            }
        }
        // WriteDefaultValues();
        Tilemap tilemap = PullTilemap();
        HtmlString tilemapString = new(tilemap.ToJson());

        // Now grab assets
        AssetList assetList = new();
        assetList.PullFromFile();
        HtmlString assetListString = new(assetList.ToJson());

        // How to deserialize json, goddamn:
		// List<List<List<string>>> json = JsonSerializer.Deserialize<List<List<List<string>>>>((string)tilemapString);
        // Console.WriteLine(tilemapString);
        IndexViewModel viewModel = new IndexViewModel
        {
            TileMapString = tilemapString,
            AssetList = assetListString,
            User = user,
            IsAdmin = isAdmin,
            Duty = duty,
            Leisure = leisure
        };
		return View(viewModel);
    }


	[HttpPost]
    public IActionResult EditMap(string id, string transferForm)
    {
        Console.Write("Post submitted: ");
        Console.WriteLine(transferForm);

        Tilemap tilemap = new(transferForm);
        tilemap.SaveToFile();
        return RedirectToAction("Index", "Map");
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel loginViewModel)
    {
        // We do this the proper way, adding an identity that is tied into the program properly

        if (!VerifyLogin(loginViewModel)) {
            ModelState.AddModelError(nameof(loginViewModel.Username), "Username not found");
            return RedirectToAction("Index", "Map");
        }

        // Create the claimed attributes about the user
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, loginViewModel.Username),
            new(ClaimTypes.Role, "Administrator")
            // First attribute can be a custom string if need be. 
        };

        // 3. Wrap claims into an Identity, specifying auth scheme name
        var claimsIdentity = new ClaimsIdentity(claims, "CookieUsernameAuth");

        // 4. Create the Principal (The actual object that becomes HttpContext.User)
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        // 5. Issue the cookie and sign the user in
        HttpContext.SignInAsync("CookieUsernameAuth", claimsPrincipal);

        //TODO: Defunct this
        // HttpContext.Session.SetString("Username", loginViewModel.Username);
        return RedirectToAction("Index", "Map");
    }

    private bool VerifyLogin(LoginViewModel loginViewModel)
    {
        return true;
    }

    public IActionResult Logout()
    {
        HttpContext.SignOutAsync();
        return RedirectToAction("Index", "Map");

    }

    public IActionResult Campsite()
    {
        return View();
    }

     public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public Tilemap PullTilemap()
    {
        Tilemap tilemap = new();
        tilemap.PullFromFile();
        return tilemap;
    }
}
