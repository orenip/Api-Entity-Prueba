using System;
using System.Collections.Generic;

namespace ApiPrueba;

public partial class AutoDTO
{
    public string Patente { get; set; } = null!;

    public string? Marca { get; set; }

    public string? Modelo { get; set; }

    public string? RutCliente { get; set; }

    //public virtual Cliente? RutClienteNavigation { get; set; }
}
