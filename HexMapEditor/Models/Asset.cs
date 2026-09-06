using System;
using System.Collections.Generic;

namespace HexMapEditor.Models;

public partial class Asset
{
    public string FileName { get; set; }

    public string DisplayName { get; set; }

    public double? Scale { get; set; }
}
