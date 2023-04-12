using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Courier.Models;

public partial class CourierContext : DbContext
{
    public CourierContext()
    {
    }

    public CourierContext(DbContextOptions<CourierContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Almacen> Almacens { get; set; }

    public virtual DbSet<CentroDistribucion> CentroDistribucions { get; set; }

    public virtual DbSet<Estatus> Estatuses { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<Paquete> Paquetes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sucursal> Sucursals { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Almacen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__almacen__3213E83FFDFBBF96");

            entity.ToTable("almacen");

            entity.Property(e => e.Id)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.Direccion).HasColumnName("direccion");
            entity.Property(e => e.Estatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("estatus");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasOne(d => d.EstatusNavigation).WithMany(p => p.Almacens)
                .HasForeignKey(d => d.Estatus)
                .HasConstraintName("FK_almacen_estatus1");
        });

        modelBuilder.Entity<CentroDistribucion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__centroDi__3213E83FFF9CAD1C");

            entity.ToTable("centroDistribucion");

            entity.Property(e => e.Id)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.Direccion)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.Estatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("estatus");
            entity.Property(e => e.Nombre).HasColumnName("nombre");

            entity.HasOne(d => d.EstatusNavigation).WithMany(p => p.CentroDistribucions)
                .HasForeignKey(d => d.Estatus)
                .HasConstraintName("FK_centroDistribucion_estatus1");
        });

        modelBuilder.Entity<Estatus>(entity =>
        {
            entity.HasKey(e => e.Estatus1).HasName("PK__estatus__7C6BAF5EBDB8998A");

            entity.ToTable("estatus");

            entity.Property(e => e.Estatus1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("estatus");
            entity.Property(e => e.Decripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("decripcion");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.ToTable("factura");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("estatus");
            entity.Property(e => e.FechaGeneracion)
                .HasColumnType("datetime")
                .HasColumnName("fechaGeneracion");
            entity.Property(e => e.FechaPago).HasColumnType("datetime");
            entity.Property(e => e.IdPaquete).HasColumnName("id_paquete");
            entity.Property(e => e.IdUsuario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_usuario");
            entity.Property(e => e.Total).HasColumnName("total");

            entity.HasOne(d => d.EstatusNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.Estatus)
                .HasConstraintName("FK_factura_estatus");

            entity.HasOne(d => d.IdPaqueteNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.IdPaquete)
                .HasConstraintName("FK_factura_paquetes");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_factura_usuarios");
        });

        modelBuilder.Entity<Paquete>(entity =>
        {
            entity.HasKey(e => e.IdPaquete).HasName("PK__paquetes__609C3BCB9E041EAA");

            entity.ToTable("paquetes");

            entity.Property(e => e.IdPaquete).HasColumnName("id_paquete");
            entity.Property(e => e.Contenido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("contenido");
            entity.Property(e => e.Estatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("estatus");
            entity.Property(e => e.IdUsuario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_usuario");
            entity.Property(e => e.Peso).HasColumnName("peso");
            entity.Property(e => e.Remitente)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("remitente");

            entity.HasOne(d => d.EstatusNavigation).WithMany(p => p.Paquetes)
                .HasForeignKey(d => d.Estatus)
                .HasConstraintName("FK_paquetes_estatus");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Paquetes)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_paquetes_usuarios");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Rol).HasName("PK__roles__C2B79D276F717221");

            entity.ToTable("roles");

            entity.Property(e => e.Rol)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("rol");
            entity.Property(e => e.Decripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("decripcion");
            entity.Property(e => e.Estatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("estatus");

            entity.HasOne(d => d.EstatusNavigation).WithMany(p => p.Roles)
                .HasForeignKey(d => d.Estatus)
                .HasConstraintName("FK_roles_estatus");
        });

        modelBuilder.Entity<Sucursal>(entity =>
        {
            entity.ToTable("sucursal");

            entity.Property(e => e.Id)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.Direccion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.Estatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("estatus");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasOne(d => d.EstatusNavigation).WithMany(p => p.Sucursals)
                .HasForeignKey(d => d.Estatus)
                .HasConstraintName("FK_sucursal_estatus");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Cedula).HasName("PK__usuarios__415B7BE4D5F4264A");

            entity.ToTable("usuarios");

            entity.Property(e => e.Cedula)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cedula");
            entity.Property(e => e.Apellido)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Celular).HasColumnName("celular");
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.Estatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("estatus");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Rol)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("rol");
            entity.Property(e => e.Sexo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("sexo");
            entity.Property(e => e.Sucursal)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("sucursal");
            entity.Property(e => e.Telefono).HasColumnName("telefono");

            entity.HasOne(d => d.EstatusNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.Estatus)
                .HasConstraintName("FK_usuarios_estatus");

            entity.HasOne(d => d.RolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.Rol)
                .HasConstraintName("FK_usuarios_roles");

            entity.HasOne(d => d.SucursalNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.Sucursal)
                .HasConstraintName("FK_usuarios_sucursal");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
