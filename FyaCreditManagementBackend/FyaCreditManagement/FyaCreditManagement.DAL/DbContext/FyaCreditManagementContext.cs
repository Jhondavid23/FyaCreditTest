using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FyaCreditManagement.Model;

public partial class FyaCreditManagementContext : DbContext
{
    public FyaCreditManagementContext()
    {
    }

    public FyaCreditManagementContext(DbContextOptions<FyaCreditManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditoriaCredito> AuditoriaCreditos { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Comercial> Comerciales { get; set; }

    public virtual DbSet<Credito> Creditos { get; set; }

    public virtual DbSet<EstadosCredito> EstadosCreditos { get; set; }

    public virtual DbSet<LogEnvioCorreo> LogEnvioCorreos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local); DataBase=FyaCreditManagement; Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditoriaCredito>(entity =>
        {
            entity.HasKey(e => e.AuditoriaId).HasName("PK__Auditori__095694C354F54D5B");

            entity.HasIndex(e => e.CreditoId, "IX_AuditoriaCreditos_CreditoId");

            entity.HasIndex(e => e.FechaAccion, "IX_AuditoriaCreditos_FechaAccion").IsDescending();

            entity.Property(e => e.Accion).HasMaxLength(20);
            entity.Property(e => e.DireccionIp)
                .HasMaxLength(45)
                .HasColumnName("DireccionIP");
            entity.Property(e => e.FechaAccion).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Usuario).HasMaxLength(100);

            entity.HasOne(d => d.Credito).WithMany(p => p.AuditoriaCreditos)
                .HasForeignKey(d => d.CreditoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AuditoriaCreditos_Credito");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PK__Clientes__71ABD087554CC8BC");

            entity.HasIndex(e => e.Nombre, "IX_Clientes_Nombre");

            entity.HasIndex(e => e.NumeroIdentificacion, "IX_Clientes_NumeroIdentificacion");

            entity.HasIndex(e => e.NumeroIdentificacion, "UQ__Clientes__FCA68D91244C0AE4").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Ciudad).HasMaxLength(100);
            entity.Property(e => e.Direccion).HasMaxLength(300);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.FechaModificacion).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Nombre).HasMaxLength(200);
            entity.Property(e => e.NumeroIdentificacion).HasMaxLength(50);
            entity.Property(e => e.Telefono).HasMaxLength(20);
            entity.Property(e => e.TipoIdentificacion)
                .HasMaxLength(20)
                .HasDefaultValue("CC");
        });

        modelBuilder.Entity<Comercial>(entity =>
        {
            entity.HasKey(e => e.ComercialId).HasName("PK__Comercia__D82B7A48830BEBAA");

            entity.HasIndex(e => e.Email, "UQ__Comercia__A9D10534A99811F0").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.FechaModificacion).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Nombre).HasMaxLength(150);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<Credito>(entity =>
        {
            entity.HasKey(e => e.CreditoId).HasName("PK__Creditos__4FE406DDB4D4EACD");

            entity.ToTable(tb => tb.HasTrigger("tr_AuditoriaCreditos"));

            entity.HasIndex(e => e.ClienteId, "IX_Creditos_Cliente");

            entity.HasIndex(e => e.ComercialId, "IX_Creditos_Comercial");

            entity.HasIndex(e => e.EstadoId, "IX_Creditos_Estado");

            entity.HasIndex(e => e.FechaRegistro, "IX_Creditos_FechaRegistro").IsDescending();

            entity.HasIndex(e => e.ValorCredito, "IX_Creditos_ValorCredito").IsDescending();

            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.FechaModificacion).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.FechaVencimiento).HasComputedColumnSql("(dateadd(month,[PlazoMeses],[FechaAprobacion]))", false);
            entity.Property(e => e.TasaInteres).HasColumnType("decimal(5, 4)");
            entity.Property(e => e.UsuarioCreacion)
                .HasMaxLength(100)
                .HasDefaultValueSql("(suser_sname())");
            entity.Property(e => e.UsuarioModificacion)
                .HasMaxLength(100)
                .HasDefaultValueSql("(suser_sname())");
            entity.Property(e => e.ValorCredito).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ValorCuota)
                .HasComputedColumnSql("((([ValorCredito]*(([TasaInteres]/(100))/(12)))*power((1)+([TasaInteres]/(100))/(12),[PlazoMeses]))/(power((1)+([TasaInteres]/(100))/(12),[PlazoMeses])-(1)))", true)
                .HasColumnType("decimal(38, 6)");
            entity.Property(e => e.ValorTotal)
                .HasComputedColumnSql("([ValorCredito]*((1)+(([TasaInteres]/(100))*[PlazoMeses])/(12)))", true)
                .HasColumnType("decimal(38, 8)");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Creditos)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Creditos_Cliente");

            entity.HasOne(d => d.Comercial).WithMany(p => p.Creditos)
                .HasForeignKey(d => d.ComercialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Creditos_Comercial");

            entity.HasOne(d => d.Estado).WithMany(p => p.Creditos)
                .HasForeignKey(d => d.EstadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Creditos_Estado");
        });

        modelBuilder.Entity<EstadosCredito>(entity =>
        {
            entity.HasKey(e => e.EstadoId).HasName("PK__EstadosC__FEF86B00C37D0D8E");

            entity.ToTable("EstadosCredito");

            entity.HasIndex(e => e.Codigo, "UQ__EstadosC__06370DACDF93D5F1").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Codigo).HasMaxLength(20);
            entity.Property(e => e.Descripcion).HasMaxLength(100);
        });

        modelBuilder.Entity<LogEnvioCorreo>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__LogEnvio__5E548648C06E1956");

            entity.HasIndex(e => e.EstadoEnvio, "IX_LogEnvioCorreos_EstadoEnvio");

            entity.HasIndex(e => e.FechaIntento, "IX_LogEnvioCorreos_FechaIntento").IsDescending();

            entity.Property(e => e.AsuntoCorreo).HasMaxLength(200);
            entity.Property(e => e.DestinatarioEmail).HasMaxLength(100);
            entity.Property(e => e.EstadoEnvio)
                .HasMaxLength(20)
                .HasDefaultValue("PENDIENTE");
            entity.Property(e => e.FechaIntento).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.MensajeError).HasMaxLength(500);

            entity.HasOne(d => d.Credito).WithMany(p => p.LogEnvioCorreos)
                .HasForeignKey(d => d.CreditoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LogEnvioCorreos_Credito");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
