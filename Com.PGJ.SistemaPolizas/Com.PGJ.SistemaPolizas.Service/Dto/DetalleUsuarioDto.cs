using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.PGJ.SistemaPolizas.Data.Model;
using Nelibur.ObjectMapper;

namespace Com.PGJ.SistemaPolizas.Service.Dto
{
    public class DetalleUsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ApellidoMaterno { get; set; }
        public string ApellidoPaterno { get; set; }
        public System.DateTime FechaDeNacimiento { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        public int AreaId { get; set; }
        public int NumeroDeEmpleado { get; set; }
        public string AuthUserId { get; set; }

        internal static DetallesUsuarios ToUnMap(DetalleUsuarioDto dto)
        {
            if (dto == null) return null;
            TinyMapper.Bind<DetalleUsuarioDto, DetallesUsuarios>();
            DetallesUsuarios model = TinyMapper.Map<DetallesUsuarios>(dto);
            return model;
        }
    }
}
