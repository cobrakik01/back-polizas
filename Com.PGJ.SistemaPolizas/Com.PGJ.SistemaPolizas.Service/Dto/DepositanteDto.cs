using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.PGJ.SistemaPolizas.Data.Model;
using Nelibur.ObjectMapper;

namespace Com.PGJ.SistemaPolizas.Service.Dto
{
    public class DepositanteDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ApellidoMaterno { get; set; }
        public string ApellidoPaterno { get; set; }
        public int AfianzadoId { get; set; }

        public Afianzados Afianzados { get; set; }

        internal static Depositantes ToUnMap(DepositanteDto dto)
        {
            if (dto == null) return null;
            TinyMapper.Bind<DepositanteDto, Depositantes>();
            Depositantes model = TinyMapper.Map<Depositantes>(dto);
            return model;
        }
    }
}
