using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.PGJ.SistemaPolizas.Data.Model;
using Nelibur.ObjectMapper;

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

        public DepositanteDto Depositantes { get; set; }
        public DetalleUsuarioDto DetallesUsuarios { get; set; }
        public PolizaDto Polizas { get; set; }

        internal static IngresoDto ToMap(Ingresos model)
        {
            if (model == null) return null;
            try
            {
                TinyMapper.Bind<Ingresos, IngresoDto>();
                IngresoDto dto = TinyMapper.Map<IngresoDto>(model);
                return dto;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
        }
    }
}
