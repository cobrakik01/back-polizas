using Com.PGJ.SistemaPolizas.Data.Model;
using Com.PGJ.SistemaPolizas.Models;
using Com.PGJ.SistemaPolizas.Service;
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
        public async Task<List<Areas>> GetAll()
        {
            List<Areas> areas = await service.GetAllAsync();
            return areas;
        }

        [HttpGet]
        public async Task<AreasSearchViewModel> Get(int page = 1, int count = 10, string sorting = "asc", string filter = "")
        {
            int total;
            AreasSearchViewModel response = new AreasSearchViewModel();
            response.result = await service.FindByFilterAsync(out total, page, count, sorting, filter);
            response.total = total;
            return response;
        }

        // POST api/<controller>
        [Route("new")]
        [HttpPost]
        public IHttpActionResult Post([FromBody]string nombre)
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                Areas area = new Areas { Nombre = nombre };
                var result = db.Areas.Where(e => e.Nombre.Contains(area.Nombre)).FirstOrDefault();
                if (result == null)
                {
                    db.Areas.Add(area);
                    db.SaveChanges();
                    return Json(new { Message = new { Type = "success", Title = "Alta", Message = string.Format("El Area {0} se dio de alta correctamente.", area.Nombre) } });
                }
                return Json(new { Message = new { Type = "warning", Title = "Alta", Message = string.Format("El Area {0} ya se encuentra registrada.", area.Nombre) } });
            }
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
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