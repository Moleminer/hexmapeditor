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
using Microsoft.EntityFrameworkCore;

namespace HexMapEditor.Controllers;

public class TownController : Controller
{
    private readonly ILogger<TownController> _logger;
    private RGRContext _context;
    private readonly static int NANA_STOCK_FURNITURE = 8;
    private readonly static int NANA_STOCK_EQUIPMENT = 5;
    private readonly static int NANA_STOCK_MAGIC_ITEM = 5;
    private readonly static int NANA_STOCK_SCROLL = 3;
    private readonly static int NANA_STOCK_TRINKETS = 6;

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
        
        if (_context.Metadata.First().UpdateDate.CompareTo(DateTime.Today.AddDays(-2).Date)<0)
        {
            RestockShops();
            Console.WriteLine("Added to nana's store!");
        }
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

    protected async Task RestockShops()
    {
		// 1. Clear Nana's stock

#pragma warning disable EF1002 // Risk of vulnerability to SQL injection.
		await _context.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE [{nameof(NanaStock)}]");
#pragma warning restore EF1002 // Risk of vulnerability to SQL injection.

        // 2. Get the lists of each category. 
            // Attribute
            // RandomItem
            // ItemTypes:
            // (1, 'Equipment'),
            // (2, 'Magic Item'),
            // (3, 'Scroll'),
            // (4, 'Trinket'),
            // (5, 'Furniture'),
            // (6, 'Pet'),
            // (7, 'Misc');

        StockStoreForType(1, NANA_STOCK_EQUIPMENT);
        StockStoreForType(5, NANA_STOCK_FURNITURE);
        StockStoreForType(2, NANA_STOCK_MAGIC_ITEM);
        Metadata metadata = await _context.Metadata.FirstAsync();
        metadata.UpdateDate = DateTime.Now;
        
        await _context.SaveChangesAsync();
    }

    private void StockStoreForType(int itemTypeID, int numElements)
    {
        Random rand = new();
        List<HexMapEditor.Models.Attribute> attributes = _context.Attributes.Where(x => x.ItemTypeId == itemTypeID).ToList();
        List<RandomItem> items = _context.RandomItems.Where(x => x.ItemTypeId == itemTypeID).ToList();
        for (int i = 0; i < numElements; i++)
        {
            RandomItem item = items.ElementAt(rand.Next(items.Count));
            HexMapEditor.Models.Attribute attribute = attributes.ElementAt(rand.Next(attributes.Count));
            _context.NanaStocks.Add(new NanaStock
            {
                ItemId = item.ItemId,
                AttributeID = attribute.AttributeId,
                ItemName = $"{attribute.AttributeValue} {item.ItemName}",
                ItemDescription = item.ItemDescription + attribute.AttributeDescription,
                Price = Math.Round((double)(item.Price * attribute.PriceModifier), 3)
            });
        }
    }
}
