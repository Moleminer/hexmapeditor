using System;
using System.Collections.Generic;

namespace HexMapEditor.Models;

public partial class ItemType
{
    public int ItemTypeId { get; set; }

    public string ItemTypeValue { get; set; }

    public virtual ICollection<Attribute> Attributes { get; set; } = new List<Attribute>();

    public virtual ICollection<RandomItem> RandomItems { get; set; } = new List<RandomItem>();
}
