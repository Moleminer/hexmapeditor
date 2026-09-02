using System;
using System.Collections.Generic;

namespace HexMapEditor;

public partial class RandomItem
{
    public int ItemId { get; set; }

    public string ItemName { get; set; }

    public int? ItemTypeId { get; set; }

    public string ItemDescription { get; set; }

    public int? AttributeId { get; set; }

    public virtual Attribute Attribute { get; set; }

    public virtual ItemType ItemType { get; set; }
}
