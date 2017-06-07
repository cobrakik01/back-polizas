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
    [RoutePrefix("api/ministerios")]
    public class MinisteriosController : ApiController
    {
        private MinisteriosService service;

        public MinisteriosController()
        {
            service = new MinisteriosService();
        }

        [Route("all")]
        [HttpGet]
        public async Task<List<MinisterioPublicoDto>> GetAll()
        {
            List<MinisterioPublicoDto> list = await service.GetAllAsync();
            return list;
        }

        [Route()]
        [HttpGet]
        public async Task<SearchResultViewModel> Search(int page = 1, int count = 10, string sorting = "asc", string filter = "")
        {
            SearchResultViewModel response = new SearchResultViewModel();
            List<MinisterioPublicoDto> list = await service.FindByFilterAsync(sorting, filter);
            response.total = list.Count();
            response.result = list.ToPagedList(page, count);
            return response;
        }

        [Route()]
        [HttpPost]
        public IHttpActionResult Save(MinisterioPublicoDto dto)
        {
            try
            {
                MinisterioPublicoDto ministerio = service.Save(dto.Nombre);
                if (ministerio != null)
                    return Ok(new { Message = new { Type = "success", Title = "Alta", Message = string.Format("El ministerio publico {0} se dio de alta correctamente.", ministerio.Nombre) } });
            }
            catch (Exception ex)
            {
                return Ok(new { Message = new { Type = "warning", Title = "Alta", Message = string.Format(ex.Message) } });
            }
            return StatusCode(HttpStatusCode.NotFound);
        }

        [Route()]
        [HttpPatch]
        public IHttpActionResult Update(MinisterioPublicoDto area)
        {
            try
            {
                MinisterioPublicoDto dto = service.Update(area);
                if (dto != null)
                    return Json(new { Message = new { Type = "success", Title = "Editar", Message = string.Format("El ministerio publico se actualizo correctamente.") } });
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
                    return Json(new { Message = new { Type = "success", Title = "Eliminar", Message = string.Format("El ministerio publico fue eliminada correctamente.") } });
                else
                    return Json(new { Message = new { Type = "success", Title = "Eliminar", Message = string.Format("El ministerio publico no pudo ser eliminada.") } });
            }
            catch (Exception ex)
            {
                return Json(new { Message = new { Type = "warning", Title = "", Message = ex.Message } });
            }
        }
    }
}