﻿using Com.PGJ.SistemaPolizas.Service;
using Com.PGJ.SistemaPolizas.Service.Dto;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    }

    public class PolizasCreateRequest
    {
        public DepositanteDto depositante { get; set; }
        public AfianzadoDto afianzado { get; set; }
        public PolizaDto poliza { get; set; }
        public AfianzadoraDto afianzadora { get; set; }
        public decimal cantidad { get; set; }
    }
}