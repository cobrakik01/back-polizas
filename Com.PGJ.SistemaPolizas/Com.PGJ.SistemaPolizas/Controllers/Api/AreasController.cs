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

        [Route()]
        [HttpPost]
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
                return Ok(new { Message = new { Type = "warning", Title = "", Message = string.Format(ex.Message) } });
            }
            return StatusCode(HttpStatusCode.NotFound);
        }

        // DELETE api/areas/5
        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                if (service.Delete(id))
                    return Json(new { Message = new { Type = "success", Title = "Eliminar", Message = string.Format("El Área fue eliminada correctamente.") } });
                else
                    return Json(new { Message = new { Type = "success", Title = "Eliminar", Message = string.Format("El Área pudo ser eliminada.") } });
            }
            catch (Exception ex)
            {
                return Json(new { Message = new { Type = "warning", Title = "", Message = ex.Message } });
            }
        }
    }
}