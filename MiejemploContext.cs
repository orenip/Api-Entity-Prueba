using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ApiPrueba;

public partial class MiejemploContext : DbContext
{
    public MiejemploContext()
    {
    }

    public MiejemploContext(DbContextOptions<MiejemploContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Auto> Autos { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.,143;initial catalog=miejemplo;User=sa;Password=Basedatos.Docker1;Max Pool Size=500;Encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auto>(entity =>
        {
            entity.HasKey(e => e.Patente);

            entity.ToTable("Auto");

            entity.Property(e => e.Patente)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("patente");
            entity.Property(e => e.Marca)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("marca");
            entity.Property(e => e.Modelo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("modelo");
            entity.Property(e => e.RutCliente)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("rutCliente");

            entity.HasOne(d => d.RutClienteNavigation).WithMany(p => p.Autos)
                .HasForeignKey(d => d.RutCliente)
                .HasConstraintName("FK_Auto_Cliente");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Rut);

            entity.ToTable("Cliente");

            entity.Property(e => e.Rut)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("rut");
            entity.Property(e => e.Apellido)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Clave)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("clave");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
