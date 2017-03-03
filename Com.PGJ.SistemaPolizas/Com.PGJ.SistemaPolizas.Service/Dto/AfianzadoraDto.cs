using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.PGJ.SistemaPolizas.Data.Model;
using Nelibur.ObjectMapper;

namespace Com.PGJ.SistemaPolizas.Service.Dto
{
    public class AfianzadoraDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        internal static List<AfianzadoraDto> ToMap(List<Afianzadoras> listModel)
        {
            return (from a in listModel select ToMap(a)).ToList();
        }

        internal static AfianzadoraDto ToMap(Afianzadoras model)
        {
            if (model == null) return null;
            TinyMapper.Bind<Afianzadoras, AfianzadoraDto>();
            AfianzadoraDto dto = TinyMapper.Map<AfianzadoraDto>(model);
            return dto;
        }

        internal static Afianzadoras ToUnMap(AfianzadoraDto dto)
        {
            if (dto == null) return null;
            TinyMapper.Bind<AfianzadoraDto, Afianzadoras>();
            Afianzadoras model = TinyMapper.Map<Afianzadoras>(dto);
            return model;
        }
    }
}