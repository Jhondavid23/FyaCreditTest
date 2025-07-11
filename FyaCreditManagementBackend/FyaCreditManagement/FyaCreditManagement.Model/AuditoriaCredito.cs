using System;
using System.Collections.Generic;

namespace FyaCreditManagement.Model;

public partial class AuditoriaCredito
{
    public int AuditoriaId { get; set; }

    public int CreditoId { get; set; }

    public string Accion { get; set; } = null!;

    public string? ValoresAnteriores { get; set; }

    public string ValoresNuevos { get; set; } = null!;

    public string Usuario { get; set; } = null!;

    public DateTime FechaAccion { get; set; }

    public string? DireccionIp { get; set; }

    public virtual Credito Credito { get; set; } = null!;
}
