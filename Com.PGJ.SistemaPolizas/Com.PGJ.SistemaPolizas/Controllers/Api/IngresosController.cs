using Com.PGJ.SistemaPolizas.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Com.PGJ.SistemaPolizas.Controllers.Api
{
    [Authorize]
    [RoutePrefix("api/ingresos")]
    public class IngresosController : ApiController
    {
        private IngresosService service;

        public IngresosController() : base()
        {
            service = new IngresosService();
        }

        [HttpGet]
        [Route("total")]
        public IHttpActionResult Total()
        {
            decimal total = service.GetTotalIngresos();
            return Ok(total);
        }
    }
}