using System;
using System.Collections.Generic;

namespace HexMapEditor;

public partial class Attribute
{
    public int AttributeId { get; set; }

    public string AttributeValue { get; set; }

    public int? ItemTypeId { get; set; }

    public string AttributeDescription { get; set; }

    public virtual ItemType ItemType { get; set; }

    public virtual ICollection<RandomItem> RandomItems { get; set; } = new List<RandomItem>();
}
