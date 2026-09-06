using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HexMapEditor.Models;

public partial class NanaStock
{
    [Key]
    public int StockId { get; set; }

    public int? ItemId { get; set; }

    public int? AttributeID {get; set;}

    public string ItemName {get; set;}

    public string ItemDescription { get; set; }

    public double? Price { get; set; }
}
