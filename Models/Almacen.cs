using System;
using System.Collections.Generic;

namespace Courier.Models;

public partial class Almacen
{
    public string Id { get; set; } = null!;

    public int? Direccion { get; set; }

    public string? Estatus { get; set; }

    public string? Nombre { get; set; }

    public virtual Estatus? EstatusNavigation { get; set; }
}
