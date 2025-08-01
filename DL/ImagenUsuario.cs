using System;
using System.Collections.Generic;

namespace DL;

public partial class ImagenUsuario
{
    public int IdImagenUsuario { get; set; }

    public int? IdUsuario { get; set; }

    public byte[]? Imagen { get; set; }

    public string? Descripcion { get; set; }

    public string? Nombre { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
