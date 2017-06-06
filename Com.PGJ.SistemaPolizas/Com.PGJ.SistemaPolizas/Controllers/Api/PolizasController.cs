﻿using Com.PGJ.SistemaPolizas.Models;
using Com.PGJ.SistemaPolizas.Service;
using Com.PGJ.SistemaPolizas.Service.Dto;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
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

            try
            {
                if (service.ExisteAfianzado(afianzado))
                    return Ok(new { Message = new { Type = "warning", Title = "Cuidado!", Message = string.Format("El afianzado {0} {1} {2} ya existe.", afianzado.ApellidoPaterno, afianzado.ApellidoMaterno, afianzado.Nombre) }, SingleData = new { Afianzado = service.ExisteAfianzado(afianzado), AveriguacionPrevia = service.ExisteAveriguacionPrevia(poliza.AveriguacionPrevia) } });
                if (service.ExisteAveriguacionPrevia(poliza.AveriguacionPrevia))
                    return Ok(new { Message = new { Type = "warning", Title = "Cuidado!", Message = string.Format("La averiguación previa {0} ya existe.", poliza.AveriguacionPrevia) }, SingleData = new { Afianzado = service.ExisteAfianzado(afianzado), AveriguacionPrevia = service.ExisteAveriguacionPrevia(poliza.AveriguacionPrevia) } });

                PolizaDto polizaResult = service.Save(User.Identity.GetUserId(), poliza, depositante, afianzado, afianzadora);
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
        public async Task<SearchResultViewModel> Search(string filterObject = "", int page = 1, int count = 10, string sortingField = "", string sorting = "asc")
        {
            SearchResultViewModel response = new SearchResultViewModel();
            SearchPolozasRequest objFilterObject = JsonConvert.DeserializeObject<SearchPolozasRequest>(filterObject);
            List<SearchPolizasResponse> list = await service.FindByFilterAsync(objFilterObject, sortingField, sorting);
            response.total = list.Count();
            response.result = list.ToPagedList(page, count);
            return response;
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetPoliza(int id)
        {
            PolizaResponse poliza = service.FindPolizaById(id);
            return Ok(poliza);
        }

        [HttpPatch]
        [Route()]
        public IHttpActionResult Update(PolizaDto request)
        {
            try
            {
                PolizaDto polizaModified = service.Update(request);
                if (polizaModified != null)
                    return Json(new { Message = new { Type = "success", Title = "Editar", Message = string.Format("La póliza se actualizo correctamente.") } });
            }
            catch (Exception ex)
            {
                return Ok(new { Message = new { Type = "warning", Title = "", Message = string.Format(ex.Message) } });
            }
            return StatusCode(HttpStatusCode.NotFound);
        }

        [HttpGet]
        [Route("{polizaId}/ingresos")]
        public async Task<SearchResultViewModel> SearchIngresos(int polizaId, string filterObject = "", int page = 1, int count = 10, string sortingField = "", string sorting = "asc")
        {
            SearchIngresoRequest request = JsonConvert.DeserializeObject<SearchIngresoRequest>(filterObject);
            SearchResultViewModel response = new SearchResultViewModel();
            List<IngresoDto> list = await service.FindIngresosByFilterAsync(polizaId, request, sortingField, sorting);
            response.total = list.Count();
            response.result = list.ToPagedList(page, count);
            return response;
        }

        [HttpPost]
        [Route("{polizaId}/ingresos")]
        public IHttpActionResult SaveIngresos(int polizaId, IngresoCreateRequest request)
        {
            try
            {
                service.AddIngreso(User.Identity.GetUserId(), polizaId, request);
                return Json(new { Message = new { Type = "success", Title = "Alta", Message = string.Format("El ingreso se registro correctamente.") } });
            }
            catch (Exception ex)
            {
                return Ok(new { Message = new { Type = "warning", Title = "", Message = string.Format(ex.Message) } });
            }
        }
    }
}