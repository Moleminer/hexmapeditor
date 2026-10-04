using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HexMapEditor.Models;

public partial class Metadata
{
	[Key]
    public double VersionNumber { get; set; }
    public DateTime UpdateDate { get; set; }

}
