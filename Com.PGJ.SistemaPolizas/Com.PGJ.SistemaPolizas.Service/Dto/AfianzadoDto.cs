using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.PGJ.SistemaPolizas.Data.Model;
using Nelibur.ObjectMapper;

namespace Com.PGJ.SistemaPolizas.Service.Dto
{
    public class AfianzadoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public System.DateTime FechaDeNacimiento { get; set; }

        internal static Afianzados ToUnMap(AfianzadoDto dto)
        {
            if (dto == null) return null;
            TinyMapper.Bind<AfianzadoDto, Afianzados>();
            Afianzados entity = TinyMapper.Map<Afianzados>(dto);
            return entity;
        }
    }
}
