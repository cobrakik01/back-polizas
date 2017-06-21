using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.PGJ.SistemaPolizas.Data.Model;
using Nelibur.ObjectMapper;

namespace Com.PGJ.SistemaPolizas.Service.Dto
{
    public class EgresoDto
    {
        public int Id { get; set; }
        public DateTime? FechaDeEgreso { get; set; }
        public decimal? Cantidad { get; set; }
        public int DetalleUsuarioId { get; set; }
        public string Descripcion { get; set; }
        public string StrFechaDeEgreso
        {
            set
            {
                DateTime _date;
                if (DateTime.TryParse(value, out _date))
                {
                    FechaDeEgreso = _date;
                }
            }
        }

        public DetalleUsuarioDto DetallesUsuarios { get; set; }
        public MinisterioPublicoDto MinisteriosPublicos { get; set; }

        internal static List<EgresoDto> ToMap(List<Egresos> entityList)
        {
            List<EgresoDto> dtoList = (from entity in entityList select ToMap(entity)).ToList();
            return dtoList;
        }

        internal static EgresoDto ToMap(Egresos entity)
        {
            if (entity == null) return null;
            TinyMapper.Bind<Egresos, EgresoDto>();
            EgresoDto dto = TinyMapper.Map<EgresoDto>(entity);
            return dto;
        }

        internal static Egresos ToUnMap(EgresoDto dto)
        {
            if (dto == null) return null;
            TinyMapper.Bind<EgresoDto, Egresos>();
            Egresos entity = TinyMapper.Map<Egresos>(dto);
            return entity;
        }
    }
}
