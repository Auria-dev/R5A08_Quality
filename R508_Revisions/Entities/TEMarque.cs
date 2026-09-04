using System;
using System.Collections.Generic;

namespace R508_Revisions.Entities;

public partial class TEMarque
{
    public int MrqId { get; set; }

    public string MrqNom { get; set; } = null!;

    public virtual ICollection<TEProduit> TEProduits { get; set; } = new List<TEProduit>();
}
