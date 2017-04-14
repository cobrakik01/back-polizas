﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.PGJ.SistemaPolizas.Service.Dto
{
    public class EgresoDto
    {
        public int Id { get; set; }
        public int AutoridadId { get; set; }
        public DateTime FechaDeEgreso { get; set; }
        public decimal Cantidad { get; set; }
        public int DetalleUsuarioId { get; set; }
        public string Descripcion { get; set; }

        public virtual AutoridadDto Autoridads { get; set; }
        public virtual DetalleUsuarioDto DetallesUsuarios { get; set; }
        public virtual ICollection<MinisterioPublicoDto> MinisteriosPublicos { get; set; }
    }
}