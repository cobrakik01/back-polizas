using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.PGJ.SistemaPolizas.Data.Model;
using Nelibur.ObjectMapper;

namespace Com.PGJ.SistemaPolizas.Service.Dto
{
    public class PolizaDto
    {
        public int Id { get; set; }
        public string AveriguacionPrevia { get; set; }
        public int AfianzadoraId { get; set; }
        public string Descripcion { get; set; }
        public decimal? Cantidad { get; set; }
        public System.DateTime FechaDeAlta { get; set; }

        public AfianzadoraDto Afianzadoras { get; set; }
        public AfianzadoDto Afianzado { get; set; }

        internal static Polizas ToUnMap(PolizaDto dto)
        {
            if (dto == null) return null;
            TinyMapper.Bind<PolizaDto, Polizas>();
            Polizas model = TinyMapper.Map<Polizas>(dto);
            return model;
        }

        internal static PolizaDto ToMap(Polizas model)
        {
            if (model == null) return null;
            TinyMapper.Bind<Polizas, PolizaDto>();
            PolizaDto dto = TinyMapper.Map<PolizaDto>(model);
            return dto;
        }
    }
}
