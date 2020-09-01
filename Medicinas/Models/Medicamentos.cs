using System;
using System.Collections.Generic;

namespace Medicinas.Models
{
    public partial class Medicamentos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Presentacion { get; set; }
        public int ContenidoActual { get; set; }
        public int FrecuenciaDeTomaHr { get; set; }
        public int CantidadDeToma { get; set; }
        public DateTime Caducidad { get; set; }
        public string IdUsuario { get; set; }

        public virtual AspNetUsers IdUsuarioNavigation { get; set; }
    }
}
