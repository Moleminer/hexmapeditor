using System;
using System.Collections.Generic;

namespace HexMapEditor.Models;

public partial class RandomItem
{
    public int ItemId { get; set; }

    public string ItemName { get; set; }

    public int? ItemTypeId { get; set; }

    public string ItemDescription { get; set; }

    public double? Price { get; set; }

    public virtual ItemType ItemType { get; set; }
}
