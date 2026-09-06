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
using HexMapEditor.ViewModels;
using HexMapEditor;

namespace HexMapEditor.Controllers;

public class TownController : Controller
{
    private readonly ILogger<TownController> _logger;
    private RGRContext _context;

    public TownController(ILogger<TownController> logger, RGRContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult NanasEmporium()
    {
        NanasEmporiumViewModel viewModel = new()
        {
            Stock = _context.NanaStocks.ToList()
        };
        return View(viewModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
