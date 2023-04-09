using System;
using System.Collections.Generic;

namespace Courier.Models;

public partial class CentroDistribucion
{
    public string Id { get; set; } = null!;

    public string? Direccion { get; set; }

    public int? Nombre { get; set; }

    public string? Estatus { get; set; }

    public virtual Estatus? EstatusNavigation { get; set; }
}
