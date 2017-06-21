using Com.PGJ.SistemaPolizas.Models;
using Com.PGJ.SistemaPolizas.Service;
using Com.PGJ.SistemaPolizas.Service.Dto;
using Microsoft.AspNet.Identity;
using Microsoft.Reporting.WebForms;
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
    [RoutePrefix("api/egresos")]
    public class EgresosController : ApiController
    {
        private EgresosService service;

        public EgresosController()
        {
            service = new EgresosService();
        }

        [Route("all")]
        [HttpGet]
        public async Task<List<EgresoDto>> GetAll()
        {
            List<EgresoDto> list = await service.GetAllAsync();
            return list;
        }

        [Route()]
        [HttpGet]
        public async Task<SearchResultViewModel> Search(string filterObject = "", int page = 1, int count = 10, string sortingField = "", string sorting = "asc")
        {
            SearchResultViewModel response = new SearchResultViewModel();
            EgresoDto objFilterObject = JsonConvert.DeserializeObject<EgresoDto>(filterObject);
            List<EgresoDto> list = await service.FindByFilterAsync(objFilterObject, sortingField, sorting);
            response.total = list.Count();
            response.result = list.ToPagedList(page, count);
            return response;
        }

        [Route()]
        [HttpPost]
        public IHttpActionResult Save(EgresoDto dto)
        {
            try
            {
                EgresoDto egreso = service.Save(User.Identity.GetUserId(), dto);
                if (egreso != null)
                    return Ok(new { Message = new { Type = "success", Title = "Alta", Message = string.Format("El egreso se dio de alta correctamente.") } });
            }
            catch (Exception ex)
            {
                return Ok(new { Message = new { Type = "warning", Title = "Alta", Message = string.Format(ex.Message) } });
            }
            return StatusCode(HttpStatusCode.NotFound);
        }

        [HttpGet]
        [Route("total")]
        public IHttpActionResult Total()
        {
            decimal total = service.GetTotalEgresos();
            return Ok(total);
        }

        [HttpGet]
        [Route("{anio}/total")]
        public IHttpActionResult Total(string anio)
        {
            decimal total = service.GetTotalEgresos(anio);
            return Ok(total);
        }

        [Route("reporte/{anio}")]
        [HttpGet]
        public HttpResponseMessage Search(string anio)
        {
            Reports.ReportsDataSetTableAdapters.EgresosReportTableAdapter ss = new Reports.ReportsDataSetTableAdapters.EgresosReportTableAdapter();
            List<Reports.ReportsDataSet.EgresosReportRow> list = ss.GetDataByAnio(anio).ToList();
            string urlArchivo = System.Web.Hosting.HostingEnvironment.MapPath("~/Reports/EgresosReport.rdlc");
            ReportViewer reporte = new ReportViewer();
            reporte.Reset();
            reporte.LocalReport.ReportPath = urlArchivo;
            reporte.LocalReport.DataSources.Add(new ReportDataSource("EgresosDataSet", list));
            reporte.LocalReport.SetParameters(new ReportParameter("AnioEgresos", anio));
            reporte.LocalReport.Refresh();

            string fileName = "egresos" + anio + ".pdf";
            byte[] file = reporte.LocalReport.Render("PDF");
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(file.ToArray());
            result.Content.Headers.Add("x-filename", fileName);
            result.Content.Headers.Add("access-control-expose-headers", new[] { "x-filename" });
            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = fileName;
            result.StatusCode = HttpStatusCode.OK;
            return result;
        }
    }
}