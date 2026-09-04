using System;
using System.Collections.Generic;

namespace R508_Revisions.Entities;

public partial class TETypeproduit
{
    public int TppId { get; set; }

    public string TppNom { get; set; } = null!;

    public virtual ICollection<TEProduit> TEProduits { get; set; } = new List<TEProduit>();
}
