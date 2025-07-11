using System;
using System.Collections.Generic;

namespace FyaCreditManagement.Model;

public partial class LogEnvioCorreo
{
    public int LogId { get; set; }

    public int CreditoId { get; set; }

    public string DestinatarioEmail { get; set; } = null!;

    public string AsuntoCorreo { get; set; } = null!;

    public string CuerpoCorreo { get; set; } = null!;

    public string EstadoEnvio { get; set; } = null!;

    public string? MensajeError { get; set; }

    public DateTime FechaIntento { get; set; }

    public DateTime? FechaEnvio { get; set; }

    public virtual Credito Credito { get; set; } = null!;
}
