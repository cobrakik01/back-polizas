using Com.PGJ.SistemaPolizas.Models;
using Com.PGJ.SistemaPolizas.Service;
using Com.PGJ.SistemaPolizas.Service.Dto;
using Microsoft.AspNet.Identity;
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
    [Authorize]
    [RoutePrefix("api/polizas")]
    public class PolizasController : ApiController
    {

        private PolizasService service;

        public PolizasController() : base()
        {
            service = new PolizasService();
        }

        [Route()]
        [HttpPost]
        public IHttpActionResult Save(PolizasCreateRequest request)
        {
            DepositanteDto depositante = request.depositante;
            AfianzadoDto afianzado = request.afianzado;
            PolizaDto poliza = request.poliza;
            AfianzadoraDto afianzadora = request.afianzadora;
            decimal cantidad = request.cantidad;

            try
            {
                if (service.ExisteAfianzado(afianzado))
                    return Ok(new { Message = new { Type = "warning", Title = "Cuidado!", Message = string.Format("El afianzado {0} {1} {2} ya existe.", afianzado.ApellidoPaterno, afianzado.ApellidoMaterno, afianzado.Nombre) }, SingleData = new { Afianzado = service.ExisteAfianzado(afianzado), AveriguacionPrevia = service.ExisteAveriguacionPrevia(poliza.AveriguacionPrevia) } });
                if (service.ExisteAveriguacionPrevia(poliza.AveriguacionPrevia))
                    return Ok(new { Message = new { Type = "warning", Title = "Cuidado!", Message = string.Format("La averiguación previa {0} ya existe.", poliza.AveriguacionPrevia) }, SingleData = new { Afianzado = service.ExisteAfianzado(afianzado), AveriguacionPrevia = service.ExisteAveriguacionPrevia(poliza.AveriguacionPrevia) } });

                PolizaDto polizaResult = service.Save(User.Identity.GetUserId(), poliza, depositante, afianzado, afianzadora, cantidad);
                if (polizaResult != null)
                    return Ok(new { Message = new { Type = "success", Title = "Alta", Message = string.Format("La Poliza se dio de alta correctamente.") } });
            }
            catch (Exception ex)
            {
                return Ok(new { Message = new { Type = "warning", Title = "Alta", Message = string.Format(ex.Message) } });
            }
            return StatusCode(HttpStatusCode.NotFound);
        }

        [Route()]
        [HttpGet]
        public async Task<SearchResultViewModel> Search(SearchPolozasRequest filterObject, int page = 1, int count = 10, string sortingField = "", string sorting = "asc")
        {
            SearchResultViewModel response = new SearchResultViewModel();
            List<SearchPolizasResponse> list = await service.FindByFilterAsync(filterObject, sortingField, sorting);
            response.total = list.Count();
            response.result = list.ToPagedList(page, count);
            return response;
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetPoliza(int id)
        {
            return Ok(service.FindPolizaById(id));
        }

        [HttpGet]
        [Route("{polizaId}/ingresos")]
        //public async Task<SearchResultViewModel> Search(int polizaId, int page = 1, int count = 10, string sorting = "asc", string filter = "")
        public async Task<SearchResultViewModel> SearchIngresos(int polizaId, int page = 1, int count = 10, string sortingField = "", string sorting = "asc", string filter = "")
        {
            SearchResultViewModel response = new SearchResultViewModel();
            List<IngresoDto> list = await service.FindIngresosByFilterAsync(polizaId, filter, sortingField, sorting);
            response.total = list.Count();
            response.result = list.ToPagedList(page, count);
            return response;
        }
    }

    public partial class IngresoRequest
    {
        public decimal Cantidad { get; set; }
        public string Descripcion { get; set; }
        public System.DateTime FechaDeIngreso { get; set; }
    }
}