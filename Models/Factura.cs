using System;
using System.Collections.Generic;

namespace Courier.Models;

public partial class Factura
{
    public int Id { get; set; }

    public string? IdUsuario { get; set; }

    public int? IdPaquete { get; set; }

    public DateTime? FechaPago { get; set; }

    public DateTime? FechaGeneracion { get; set; }

    public string? Estatus { get; set; }

    public int? Total { get; set; }

    public virtual Estatus? EstatusNavigation { get; set; }

    public virtual Paquete? IdPaqueteNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
