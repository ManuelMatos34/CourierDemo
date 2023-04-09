using System;
using System.Collections.Generic;

namespace Courier.Models;

public partial class Paquete
{
    public int IdPaquete { get; set; }

    public string? IdUsuario { get; set; }

    public string? Remitente { get; set; }

    public string? Contenido { get; set; }

    public double? Peso { get; set; }

    public string? Estatus { get; set; }

    public virtual Estatus? EstatusNavigation { get; set; }

    public virtual ICollection<Factura> Facturas { get; } = new List<Factura>();

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
