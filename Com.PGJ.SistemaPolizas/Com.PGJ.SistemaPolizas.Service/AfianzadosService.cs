using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.PGJ.SistemaPolizas.Service.Dto;
using Com.PGJ.SistemaPolizas.Data.Model;

namespace Com.PGJ.SistemaPolizas.Service
{
    public class AfianzadosService
    {
        public Task<List<DepositanteDto>> FindDepositantesFilterAsync(int afianzadoId, string filter, string sorting = "asc")
        {
            return Task.FromResult(FindDepositantesFilter(afianzadoId, filter, sorting));
        }

        private List<DepositanteDto> FindDepositantesFilter(int afianzadoId, string filter, string sorting = "asc")
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                var afianzado = db.Afianzados.Where(a => a.Id == afianzadoId).FirstOrDefault();
                var depositantes = afianzado.Depositantes.Select(e => new DepositanteDto
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    ApellidoMaterno = e.ApellidoMaterno,
                    ApellidoPaterno = e.ApellidoPaterno,
                    AfianzadoId = e.AfianzadoId
                });

                depositantes = depositantes.Where(e => e.Nombre.Contains("") || e.ApellidoMaterno.Contains("") || e.ApellidoPaterno.Contains(""));
                if (filter != null)
                {
                    depositantes = depositantes.Where(e => (e.ApellidoMaterno + e.ApellidoPaterno + e.Nombre).Contains(filter));
                }

                if (sorting == "asc")
                {
                    depositantes = depositantes.OrderBy(e => e.Nombre);
                }
                else
                {
                    depositantes = depositantes.OrderByDescending(e => e.Nombre);
                }

                return depositantes.ToList();
            }
        }
    }
}
