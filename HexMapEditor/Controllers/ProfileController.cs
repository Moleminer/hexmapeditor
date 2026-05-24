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
using Microsoft.AspNetCore.Authorization;

namespace HexMapEditor.Controllers;

public class ProfileController(ILogger<ProfileController> logger) : Controller
{
    private readonly ILogger<ProfileController> _logger = logger;

    [Authorize]
    public IActionResult Index()
    {
        string user = User.Identity.Name;
        bool isAdmin = false;
        int duty = 0;
        int leisure = 0;
        
        user = user.ToLower();
        JsonNode userInfo = Users.GetUsers()[user];
        if (userInfo != null)
        {
            isAdmin = (bool)userInfo["admin"];
            duty = (int)userInfo["duty"];
            leisure = (int)userInfo["leisure"];
        }

        ProfileViewModel viewModel = new ProfileViewModel
        {
            User = user,
            IsAdmin = isAdmin,
            Duty = duty,
            Leisure = leisure
        };

        return View(viewModel);
    }
    
    [Authorize(Roles = "Administrator")]
	[HttpGet]
    public IActionResult EditMap()
    {

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

        JsonObject userRecord = TryGetUser(loginViewModel.Username);
        if (userRecord == null) {
            ModelState.AddModelError(nameof(loginViewModel.Username), "Username not found");
            return RedirectToAction("Index", "Map");
        }

        string adminStatus = ((bool)userRecord["admin"] == true)?"Administrator":"User";

        // Create the claimed attributes about the user
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, loginViewModel.Username),
            new(ClaimTypes.Role, adminStatus),
            new Claim("Duty", (string)userRecord["duty"]),
            new Claim("Leisure", (string)userRecord["leisure"])
        };

        // How to access custom claims:
        // List<Claim> claims = User.Claims.ToList();
        // string duty = claims.FirstOrDefault(c => c.Type == "Duty").Value;
        // string leisure = claims.FirstOrDefault(c => c.Type == "Leisure").Value;

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

    private static JsonObject TryGetUser(string user)
    {
        try
        {
            JsonObject j = Users.GetUsers()[user].AsObject();
            return j;
        } catch
        {
            Console.Error.WriteLine($"User ${user} could not be found in users.json");
        }
        return null;
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
