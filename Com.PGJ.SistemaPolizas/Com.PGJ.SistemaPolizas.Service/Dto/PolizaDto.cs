using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.PGJ.SistemaPolizas.Service.Dto
{
    public class PolizaDto
    {
        public int Id { get; set; }
        public string AveriguacionPrevia { get; set; }
        public int AfianzadoraId { get; set; }
        public string Descripcion { get; set; }
        public System.DateTime FechaDeAlta { get; set; }

        public virtual AfianzadoraDto Afianzadoras { get; set; }
        public virtual ICollection<AfianzadoDto> Afianzados { get; set; }
        public virtual ICollection<IngresoDto> Ingresos { get; set; }
    }
}
