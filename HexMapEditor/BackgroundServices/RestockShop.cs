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

        // TODO: SWAP TO ONLY FURNITURE
		List<HexMapEditor.Models.Attribute> attributes = context.Attributes.ToList();
        List<RandomItem> items = context.RandomItems.ToList();

        Random rand = new();
        for (int i = 0; i < NANA_STOCK_FURNITURE; i++)
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
        // List<HexMapEditor.Attribute> attributes = context.Attributes.ToList();
        
        
        // 1. Find payments that are scheduled and in the past
        // List<BillPay> billpays = await context.BillPays
        //         .Where(x => x.BillPayStatus == BILLPAYSTATUS.Scheduled)
        //         .Where(x => x.ScheduleTimeUtc < DateTime.UtcNow)
        //         .Include(x => x.Account)
        //         .ToListAsync();
        
        // // 2: Check if the account has enough money, process transaction if so. 
        
        // foreach (BillPay b in billpays) {
        //     decimal effectiveBalance = b.Account.Balance;
        //     if (b.Account.AccountType == ACCOUNTTYPE.Checking) { effectiveBalance += 500; }

        //     if (effectiveBalance >= b.Amount)
        //     {
        //         // Make transaction
        //         Transaction t = new Transaction
        //         {
        //             TransactionType = TRANSACTIONTYPE.Billpay,
        //             AccountNumber = b.AccountNumber,
        //             Amount = b.Amount,
        //             TransactionTimeUtc = DateTime.UtcNow

        //         };
        //         await context.Transactions.AddAsync(t);
        //         // Update balance
        //         b.Account.Balance -= b.Amount;

        //         // Refresh monthly transaction
        //         if (b.Period == 'M')
        //         {
        //             BillPay refreshedBill = new BillPay
        //             {
        //                 AccountNumber = b.AccountNumber,
        //                 PayeeID = b.PayeeID,
        //                 Amount = b.Amount,
        //                 ScheduleTimeUtc = b.ScheduleTimeUtc.AddMonths(1),
        //                 Period = 'M',
        //                 BillPayStatus = BILLPAYSTATUS.Scheduled
        //             };
        //             context.BillPays.Add(refreshedBill);
        //         }

        //         // Set billpay to completed
        //         b.BillPayStatus = BILLPAYSTATUS.Completed;
                
        //     } else
        //     {
        //         // Not enough money, fail
        //         b.BillPayStatus = BILLPAYSTATUS.Failed;
        //     }
        // }
        await context.SaveChangesAsync(cancellationToken);
    }
}