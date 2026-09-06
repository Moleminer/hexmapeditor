using System;
using System.Collections.Generic;

namespace HexMapEditor.Models;

public partial class User
{
    public string UserName { get; set; }

    public bool? IsAdmin { get; set; }

    public double? Gold { get; set; }

    public int? BastionTurns { get; set; }

    public bool? HasBastion { get; set; }
}
