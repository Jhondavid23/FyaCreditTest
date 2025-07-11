using System;
using System.Collections.Generic;

namespace FyaCreditManagement.Model;

public partial class Comercial
{
    public int ComercialId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Telefono { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime FechaModificacion { get; set; }

    public virtual ICollection<Credito> Creditos { get; set; } = new List<Credito>();
}
