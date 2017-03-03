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
    [RoutePrefix("api/autoridades")]
    public class AutoridadesController : ApiController
    {
        private AutoridadesService service;

        public AutoridadesController()
        {
            service = new AutoridadesService();
        }

        [Route("all")]
        [HttpGet]
        public async Task<List<AutoridadDto>> GetAll()
        {
            List<AutoridadDto> list = await service.GetAllAsync();
            return list;
        }

        [Route()]
        [HttpGet]
        public async Task<SearchResultViewModel> Search(int page = 1, int count = 10, string sorting = "asc", string filter = "")
        {
            SearchResultViewModel response = new SearchResultViewModel();
            List<AutoridadDto> list = await service.FindByFilterAsync(sorting, filter);
            response.total = list.Count();
            response.result = list.ToPagedList(page, count);
            return response;
        }

        [Route()]
        [HttpPost]
        public IHttpActionResult Save(AutoridadDto dto)
        {
            try
            {
                AutoridadDto area = service.Save(dto.Nombre);
                if (area != null)
                    return Ok(new { Message = new { Type = "success", Title = "Alta", Message = string.Format("La Autoridad {0} se dio de alta correctamente.", area.Nombre) } });
            }
            catch (Exception ex)
            {
                return Ok(new { Message = new { Type = "warning", Title = "Alta", Message = string.Format(ex.Message) } });
            }
            return StatusCode(HttpStatusCode.NotFound);
        }

        [Route()]
        [HttpPatch]
        public IHttpActionResult Update(AutoridadDto area)
        {
            try
            {
                AutoridadDto dto = service.Update(area);
                if (dto != null)
                    return Json(new { Message = new { Type = "success", Title = "Editar", Message = string.Format("La Autoridad se actualizo correctamente.") } });
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
                    return Json(new { Message = new { Type = "success", Title = "Eliminar", Message = string.Format("La Autoridad fue eliminada correctamente.") } });
                else
                    return Json(new { Message = new { Type = "success", Title = "Eliminar", Message = string.Format("La Autoridad no pudo ser eliminada.") } });
            }
            catch (Exception ex)
            {
                return Json(new { Message = new { Type = "warning", Title = "", Message = ex.Message } });
            }
        }
    }
}