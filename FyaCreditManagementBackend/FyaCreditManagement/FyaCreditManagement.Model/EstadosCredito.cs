using System;
using System.Collections.Generic;

namespace FyaCreditManagement.Model;

public partial class EstadosCredito
{
    public int EstadoId { get; set; }

    public string Codigo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual ICollection<Credito> Creditos { get; set; } = new List<Credito>();
}
