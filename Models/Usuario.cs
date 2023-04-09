using System;
using System.Collections.Generic;

namespace Courier.Models;

public partial class Usuario
{
    public string Cedula { get; set; } = null!;

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Sexo { get; set; }

    public string? Correo { get; set; }

    public int? Telefono { get; set; }

    public int? Celular { get; set; }

    public string? Password { get; set; }

    public string? Rol { get; set; }

    public string? Estatus { get; set; }

    public string? Sucursal { get; set; }

    public virtual Estatus? EstatusNavigation { get; set; }

    public virtual ICollection<Factura> Facturas { get; } = new List<Factura>();

    public virtual ICollection<Paquete> Paquetes { get; } = new List<Paquete>();

    public virtual Role? RolNavigation { get; set; }

    public virtual Sucursal? SucursalNavigation { get; set; }
}
