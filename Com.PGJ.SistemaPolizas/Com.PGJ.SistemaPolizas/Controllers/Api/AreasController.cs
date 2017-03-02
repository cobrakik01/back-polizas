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
        public async Task<List<AreaDto>> GetAll()
        {
            List<AreaDto> areas = await service.GetAllAsync();
            return areas;
        }

        [Route()]
        [HttpGet]
        public async Task<AreasSearchViewModel> Get(int page = 1, int count = 10, string sorting = "asc", string filter = "")
        {
            AreasSearchViewModel response = new AreasSearchViewModel();
            List<AreaDto> list = await service.FindByFilterAsync(sorting, filter);
            response.total = list.Count();
            response.result = list.ToPagedList(page, count);
            return response;
        }

        // POST api/<controller>
        [Route()]
        [HttpPost]
        // public IHttpActionResult Post([FromBody]string nombre)
        public IHttpActionResult Post(AreaDto dto)
        {
            try
            {
                AreaDto area = service.Save(dto.Nombre);
                if (area != null)
                    return Ok(new { Message = new { Type = "success", Title = "Alta", Message = string.Format("El Area {0} se dio de alta correctamente.", area.Nombre) } });
            }
            catch (Exception ex)
            {
                return Ok(new { Message = new { Type = "warning", Title = "Alta", Message = string.Format(ex.Message) } });
            }
            return StatusCode(HttpStatusCode.NotFound);
        }

        // GET api/<controller>/5
        [Route()]
        [HttpPatch]
        public IHttpActionResult Patch(AreaDto area)
        {
            try
            {
                AreaDto dto = service.Update(area);
                if (dto != null)
                    return Json(new { Message = new { Type = "success", Title = "Editar", Message = string.Format("El Área se actualizo correctamente.") } });
            }
            catch (Exception ex)
            {
                return Ok(new { Message = new { Type = "warning", Title = "Alta", Message = string.Format(ex.Message) } });
            }
            return StatusCode(HttpStatusCode.NotFound);
        }

        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            return Ok("Se eliminara");
        }
    }
}