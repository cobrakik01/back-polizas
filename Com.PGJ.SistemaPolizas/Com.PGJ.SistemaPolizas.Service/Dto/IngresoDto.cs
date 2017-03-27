using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.PGJ.SistemaPolizas.Service.Dto
{
    public class IngresoDto
    {
        public int Id { get; set; }
        public decimal Cantidad { get; set; }
        public int PolizaId { get; set; }
        public System.DateTime FechaDeIngreso { get; set; }
        public int DepositanteId { get; set; }
        public string Descripcion { get; set; }
        public int DetalleUsuarioId { get; set; }

        public virtual DepositanteDto Depositantes { get; set; }
        public virtual DetalleUsuarioDto DetallesUsuarios { get; set; }
        public virtual PolizaDto Polizas { get; set; }
    }
}
