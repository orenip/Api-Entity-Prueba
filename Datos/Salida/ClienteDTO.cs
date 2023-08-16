using System;
using System.Collections.Generic;

namespace ApiPrueba;

public partial class ClienteDTO
{
    public string Rut { get; set; } = null!;

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Clave { get; set; }

    //public virtual ICollection<Auto> Autos { get; set; } = new List<Auto>();
}
