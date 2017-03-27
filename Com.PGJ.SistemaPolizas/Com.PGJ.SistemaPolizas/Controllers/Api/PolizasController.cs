using Com.PGJ.SistemaPolizas.Service;
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
        public IHttpActionResult Save(
            DepositanteDto depositante,
            AfianzadoDto afianzado,
            PolizaDto poliza,
            AfianzadoraDto afianzadora,
            decimal cantidad)
        {
            try
            {
                if(service.ExisteAfianzado(afianzado))
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

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}