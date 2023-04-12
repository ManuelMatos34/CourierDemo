using System;
using System.Collections.Generic;

namespace Courier.Models;

public partial class Estatus
{
    public string Estatus1 { get; set; } = null!;

    public string? Decripcion { get; set; }

    public virtual ICollection<Almacen> Almacens { get; } = new List<Almacen>();

    public virtual ICollection<CentroDistribucion> CentroDistribucions { get; } = new List<CentroDistribucion>();

    public virtual ICollection<Factura> Facturas { get; } = new List<Factura>();

    public virtual ICollection<Paquete> Paquetes { get; } = new List<Paquete>();

    public virtual ICollection<Role> Roles { get; } = new List<Role>();

    public virtual ICollection<Sucursal> Sucursals { get; } = new List<Sucursal>();

    public virtual ICollection<Usuario> Usuarios { get; } = new List<Usuario>();
}
