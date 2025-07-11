using System;
using System.Collections.Generic;

namespace FyaCreditManagement.Model;

public partial class Credito
{
    public int CreditoId { get; set; }

    public int ClienteId { get; set; }

    public int ComercialId { get; set; }

    public int EstadoId { get; set; }

    public decimal ValorCredito { get; set; }

    public decimal TasaInteres { get; set; }

    public int PlazoMeses { get; set; }

    public decimal? ValorCuota { get; set; }

    public decimal? ValorTotal { get; set; }

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaAprobacion { get; set; }

    public DateTime? FechaVencimiento { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime FechaModificacion { get; set; }

    public string UsuarioCreacion { get; set; } = null!;

    public string UsuarioModificacion { get; set; } = null!;

    public virtual ICollection<AuditoriaCredito> AuditoriaCreditos { get; set; } = new List<AuditoriaCredito>();

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual Comercial Comercial { get; set; } = null!;

    public virtual EstadosCredito Estado { get; set; } = null!;

    public virtual ICollection<LogEnvioCorreo> LogEnvioCorreos { get; set; } = new List<LogEnvioCorreo>();
}
