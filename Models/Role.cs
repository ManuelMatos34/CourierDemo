using System;
using System.Collections.Generic;

namespace Courier.Models;

public partial class Role
{
    public string Rol { get; set; } = null!;

    public string? Decripcion { get; set; }

    public string? Estatus { get; set; }

    public virtual Estatus? EstatusNavigation { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; } = new List<Usuario>();
}
