using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.PGJ.SistemaPolizas.Data.Model;
using Nelibur.ObjectMapper;

namespace Com.PGJ.SistemaPolizas.Service.Dto
{
    public class AutoridadDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        internal static List<AutoridadDto> ToMap(List<Autoridads> listModel)
        {
            return (from a in listModel select ToMap(a)).ToList();
        }

        internal static AutoridadDto ToMap(Autoridads model)
        {
            if (model == null) return null;
            TinyMapper.Bind<Autoridads, AutoridadDto>();
            AutoridadDto dto = TinyMapper.Map<AutoridadDto>(model);
            return dto;
        }

        internal static Autoridads ToUnMap(AutoridadDto dto)
        {
            if (dto == null) return null;
            TinyMapper.Bind<AutoridadDto, Autoridads>();
            Autoridads model = TinyMapper.Map<Autoridads>(dto);
            return model;
        }
    }
}
