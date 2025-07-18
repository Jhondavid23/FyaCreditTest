﻿using System;
using System.Collections.Generic;

namespace FyaCreditManagement.Model;

public partial class Cliente
{
    public int ClienteId { get; set; }

    public string Nombre { get; set; } = null!;

    public string NumeroIdentificacion { get; set; } = null!;

    public string TipoIdentificacion { get; set; } = null!;

    public string? Email { get; set; }

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public string? Ciudad { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime FechaModificacion { get; set; }

    public virtual ICollection<Credito> Creditos { get; set; } = new List<Credito>();
}
