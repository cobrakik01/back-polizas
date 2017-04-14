using Com.PGJ.SistemaPolizas.Models;
using Com.PGJ.SistemaPolizas.Service;
using Com.PGJ.SistemaPolizas.Service.Dto;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Com.PGJ.SistemaPolizas.Controllers.Api
{
    // [Authorize]
    [RoutePrefix("api/afianzados")]
    public class AfianzadosController : ApiController
    {
        private AfianzadosService service;

        public AfianzadosController()
        {
            service = new AfianzadosService();
        }

        [Route("{afianzadoId}/test")]
        [HttpGet]
        public int get(int afianzadoId)
        {
            return afianzadoId;
        }

        [Route("{afianzadoId}/depositantes")]
        [HttpGet]
        public async Task<SearchResultViewModel> Search(int afianzadoId, int page = 1, int count = 10, string sorting = "asc", string filter = "")
        {
            SearchResultViewModel response = new SearchResultViewModel();
            List<DepositanteDto> list = await service.FindDepositantesFilterAsync(afianzadoId, filter, sorting);
            response.total = list.Count();
            response.result = list.ToPagedList(page, count);
            return response;
        }
    }
}