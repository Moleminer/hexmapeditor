using System;
using HexMapEditor;
using HexMapEditor.Data;
using HexMapEditor.Models;
using Microsoft.EntityFrameworkCore;

namespace BackgroundServices;
public class RestockShopBackgroundService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<RestockShopBackgroundService> _logger;
    private readonly static int NANA_STOCK_FURNITURE = 8;
    private readonly static int NANA_STOCK_EQUIPMENT = 5;
    private readonly static int NANA_STOCK_MAGIC_ITEM = 2;
    private readonly static int NANA_STOCK_SCROLL = 3;
    private readonly static int NANA_STOCK_TRINKETS = 6;

    public RestockShopBackgroundService(IServiceProvider services, ILogger<RestockShopBackgroundService> logger)
    {
        _logger = logger;
        _services = services;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Nana's premium restock background service is running");

        while (!cancellationToken.IsCancellationRequested)
        {
            using var scope = _services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<RGRContext>();
            await RestockShops(context, cancellationToken);
            
            _logger.LogInformation("Nana's premium background check completed, see you next week!");
            Console.WriteLine("Added to nana's store!");
            await Task.Delay(TimeSpan.FromDays(7), cancellationToken);
        }
    }

    protected async Task RestockShops(RGRContext context, CancellationToken cancellationToken)
    {
		// 1. Clear Nana's stock

#pragma warning disable EF1002 // Risk of vulnerability to SQL injection.
		await context.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE [{nameof(NanaStock)}]", cancellationToken: cancellationToken);
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

        StockStoreForType(1, NANA_STOCK_EQUIPMENT, context, cancellationToken);
        StockStoreForType(5, NANA_STOCK_FURNITURE, context, cancellationToken);

        
        await context.SaveChangesAsync(cancellationToken);
    }

    private void StockStoreForType(int itemTypeID, int numElements, RGRContext context, CancellationToken cancellationToken)
    {
        Random rand = new();
        List<HexMapEditor.Models.Attribute> attributes = context.Attributes.Where(x => x.ItemTypeId == itemTypeID).ToList();
        List<RandomItem> items = context.RandomItems.Where(x => x.ItemTypeId == itemTypeID).ToList();
        for (int i = 0; i < numElements; i++)
        {
            RandomItem item = items.ElementAt(rand.Next(items.Count));
            HexMapEditor.Models.Attribute attribute = attributes.ElementAt(rand.Next(attributes.Count));
            context.NanaStocks.Add(new NanaStock
            {
                ItemId = item.ItemId,
                AttributeID = attribute.AttributeId,
                ItemName = $"{attribute.AttributeValue} {item.ItemName}",
                ItemDescription = item.ItemDescription + attribute.AttributeDescription,
                Price = item.Price * attribute.PriceModifier
            });
        }
    }
}