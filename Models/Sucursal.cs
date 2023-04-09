using System;
using System.Collections.Generic;

namespace Courier.Models;

public partial class Sucursal
{
    public string Id { get; set; } = null!;

    public string? Nombre { get; set; }

    public string? Direccion { get; set; }

    public string? Estatus { get; set; }

    public virtual Estatus? EstatusNavigation { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; } = new List<Usuario>();
}
