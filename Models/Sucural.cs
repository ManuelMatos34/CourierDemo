using System;
using System.Collections.Generic;

namespace Courier.Models;

public partial class Sucural
{
    public string Id { get; set; } = null!;

    public string? IdCentroDistribucion { get; set; }

    public int? IdPaquete { get; set; }

    public DateOnly? Fecha { get; set; }

    public string? Estatus { get; set; }

    public virtual Estatus? EstatusNavigation { get; set; }

    public virtual CentroDistribucion? IdCentroDistribucionNavigation { get; set; }

    public virtual Paquete? IdPaqueteNavigation { get; set; }
}
