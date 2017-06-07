using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.PGJ.SistemaPolizas.Data.Model;
using Nelibur.ObjectMapper;

namespace Com.PGJ.SistemaPolizas.Service.Dto
{
    public class MinisterioPublicoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int? AutoridadId { get; set; }
        public int? EgresoId { get; set; }

        internal static List<MinisterioPublicoDto> ToMap(List<MinisteriosPublicos> entityList) {
            List<MinisterioPublicoDto> list = (from entity in entityList select ToMap(entity)).ToList();
            return list;
        }

        internal static MinisterioPublicoDto ToMap(MinisteriosPublicos entity)
        {
            if (entity == null) return null;
            TinyMapper.Bind<MinisteriosPublicos, MinisterioPublicoDto>();
            MinisterioPublicoDto dto = TinyMapper.Map<MinisterioPublicoDto>(entity);
            return dto;
        }

        internal static MinisteriosPublicos ToUnMap(MinisterioPublicoDto dto)
        {
            if (dto == null) return null;
            TinyMapper.Bind<MinisterioPublicoDto, MinisteriosPublicos>();
            MinisteriosPublicos entity = TinyMapper.Map<MinisteriosPublicos>(dto);
            return entity;
        }
    }
}
