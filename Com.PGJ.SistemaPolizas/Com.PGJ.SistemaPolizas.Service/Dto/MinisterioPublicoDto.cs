using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.PGJ.SistemaPolizas.Service.Dto
{
    public class MinisterioPublicoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int AutoridadId { get; set; }
        public int EgresoId { get; set; }
        // public Nullable<int> EgresoId { get; set; }

        public AutoridadDto Autoridads { get; set; }
        public EgresoDto Egresos { get; set; }
    }
}
