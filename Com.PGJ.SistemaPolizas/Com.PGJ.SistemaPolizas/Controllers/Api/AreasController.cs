using Com.PGJ.SistemaPolizas.Data.Model;
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
    [RoutePrefix("api/areas")]
    public class AreasController : ApiController
    {
        private AreasService service;

        public AreasController()
        {
            service = new AreasService();
        }
        // GET api/<controller>
        [Route("all")]
        [HttpGet]
        public List<Areas> GetAll()
        {
            return service.GetAll();
        }

        [HttpGet]
        public IHttpActionResult Get(int page = 1, int count = 10, string sorting = "asc", string filter = "")
        {
            return Ok();
            //return Ok(new { result = data, total = cnt });
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