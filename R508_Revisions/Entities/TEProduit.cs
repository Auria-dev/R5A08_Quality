using System;
using System.Collections.Generic;

namespace R508_Revisions.Entities;

public partial class TEProduit
{
    public int PrdProduit { get; set; }

    public string PrdNom { get; set; } = null!;

    public string PrdDescription { get; set; } = null!;

    public string PrdNomphoto { get; set; } = null!;

    public string PrdUriphoto { get; set; } = null!;

    public int PrdIdtype { get; set; }

    public int PrdIdmarque { get; set; }

    public int PrdStockreel { get; set; }

    public int PrdStockmin { get; set; }

    public int PrdStockmax { get; set; }

    public virtual TETypeproduit PrdIdmarqueNavigation { get; set; } = null!;

    public virtual TEMarque PrdIdtypeNavigation { get; set; } = null!;
}
