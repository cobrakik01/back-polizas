using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.PGJ.SistemaPolizas.Data.Model;
using Nelibur.ObjectMapper;

namespace Com.PGJ.SistemaPolizas.Service.Dto
{
    public class AreaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        internal static AreaDto ToMap(Areas model)
        {
            if (model == null) return null;
            TinyMapper.Bind<Areas, AreaDto>();
            AreaDto dto = TinyMapper.Map<AreaDto>(model);
            return dto;
        }

        internal static Areas ToUnMap(AreaDto dto)
        {
            if (dto == null) return null;
            TinyMapper.Bind<AreaDto, Areas>();
            Areas model = TinyMapper.Map<Areas>(dto);
            return model;
        }

        internal static List<AreaDto> ToMap(List<Areas> list)
        {
            return (from a in list select ToMap(a)).ToList();
        }
    }
}
