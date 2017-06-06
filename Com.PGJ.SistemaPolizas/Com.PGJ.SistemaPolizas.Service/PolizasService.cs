using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.PGJ.SistemaPolizas.Service.Dto;
using Com.PGJ.SistemaPolizas.Data.Model;

namespace Com.PGJ.SistemaPolizas.Service
{
    public class PolizasService
    {

        public PolizaDto Save(string userId, PolizaDto _polizaDto, DepositanteDto depositanteDto, AfianzadoDto afianzadoDto, AfianzadoraDto afianzadoraDto)
        {
            DateTime _createdAt = DateTime.Now;
            Polizas poliza = PolizaDto.ToUnMap(_polizaDto);
            Afianzados afianzado = AfianzadoDto.ToUnMap(afianzadoDto);
            Depositantes depositante = DepositanteDto.ToUnMap(depositanteDto);

            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                using (System.Data.Entity.DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    Afianzadoras afianzadora = db.Afianzadoras.Where(a => a.Id == afianzadoraDto.Id).FirstOrDefault();
                    DetallesUsuarios _currentUser = db.DetallesUsuarios.Where(e => e.AuthUserId == userId).FirstOrDefault();

                    db.Afianzados.Add(afianzado);
                    poliza.FechaDeAlta = _createdAt;

                    /*
                    Ingresos ingreso = new Ingresos();
                    ingreso.Cantidad = cantidad;
                    ingreso.FechaDeIngreso = _createdAt;
                    ingreso.Descripcion = poliza.Descripcion;
                    */

                    afianzadora.Polizas.Add(poliza);
                    afianzado.Polizas.Add(poliza);
                    afianzado.Depositantes.Add(depositante);
                    /*
                    poliza.Ingresos.Add(ingreso);
                    depositante.Ingresos.Add(ingreso);
                    _currentUser.Ingresos.Add(ingreso);
                    */

                    int n = db.SaveChanges();
                    transaction.Commit();
                    return _polizaDto;
                }
            }
        }

        private T CheckContext<T>(Func<PGJSistemaPolizasEntities, T> fn, PGJSistemaPolizasEntities db = null)
        {
            if (db == null)
                using (db = new PGJSistemaPolizasEntities())
                    return fn(db);
            else
                return fn(db);
        }

        private bool ExisteDepositante(Depositantes depositante, PGJSistemaPolizasEntities db = null)
        {
            return CheckContext((_db) => null != _db.Depositantes.Where(e => e.Nombre.Contains(depositante.Nombre) && e.ApellidoPaterno.Contains(depositante.ApellidoPaterno) && e.ApellidoMaterno.Contains(depositante.ApellidoMaterno)).FirstOrDefault(), db);
        }

        public bool ExisteAveriguacionPrevia(string averiguacionPrevia, PGJSistemaPolizasEntities _db = null)
        {
            return CheckContext((db) => null != db.Polizas.Where(e => e.AveriguacionPrevia == averiguacionPrevia).FirstOrDefault(), _db);
        }

        public Task<List<SearchPolizasResponse>> FindByFilterAsync(SearchPolozasRequest filterObject, string sortingField, string sorting = "asc")
        {
            return Task.FromResult(FindByFilter(filterObject, sortingField, sorting));
        }

        public PolizaResponse FindPolizaById(int id)
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                Polizas poliza = db.Polizas.Where(p => p.Id == id).FirstOrDefault();
                PolizaResponse r = new PolizaResponse();
                r.Poliza = PolizaDto.ToMap(poliza);
                r.TotalIngresos = poliza.Ingresos.Sum(i => i.Cantidad);
                r.TotalIngresos = r.TotalIngresos.GetValueOrDefault();
                return r;
            }
        }

        public PolizaDto Update(PolizaDto request)
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                Polizas poliza = PolizaDto.ToUnMap(request);
                poliza.AfianzadoraId = poliza.Afianzadoras.Id;
                db.Entry<Polizas>(poliza).State = System.Data.Entity.EntityState.Modified;
                int nModified = db.SaveChanges();
                if (nModified > 0)
                {
                    return request;
                }
            }
            return null;
        }

        public Task<List<IngresoDto>> FindIngresosByFilterAsync(int polizaId, SearchIngresoRequest request = null, string sortingField = "", string sorting = "asc")
        {
            return Task.FromResult(FindIngresosByFilter(polizaId, request, sortingField, sorting));
        }

        private List<IngresoDto> FindIngresosByFilter(int polizaId, SearchIngresoRequest request = null, string sortingField = "", string sorting = "asc")
        {
            List<IngresoDto> list = new List<IngresoDto>();
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                var poliza = db.Polizas.Where(p => p.Id == polizaId).FirstOrDefault();
                if (poliza != null)
                {
                    var query = poliza.Ingresos.Select(e => e);
                    if (request != null)
                    {
                        if (request.Cantidad != null)
                            query = query.Where(e => e.Cantidad.ToString().Contains(request.Cantidad.ToString()));
                        if (!string.IsNullOrEmpty(request.Depositante))
                            query = query.Where(e => e.Depositantes != null && (e.Depositantes.ApellidoMaterno + e.Depositantes.ApellidoPaterno + e.Depositantes.Nombre).ToLower().Contains(request.Depositante.ToLower()));
                        if (!string.IsNullOrEmpty(request.Descripcion))
                            query = query.Where(e => e.Descripcion.ToLower().Contains(request.Descripcion.ToLower()));
                        if (!string.IsNullOrEmpty(request.FechaDeIngreso))
                            query = query.Where(e => e.FechaDeIngreso.ToString().Contains(request.FechaDeIngreso));
                    }
                    var listIngresos = query.ToList();
                    list = listIngresos.Select(i => IngresoDto.ToMap(i)).ToList();
                }
            }
            return list;
        }

        public bool AddIngreso(string userId, int polizaId, IngresoCreateRequest request)
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                using (System.Data.Entity.DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    DetallesUsuarios currentUserDetails = db.DetallesUsuarios.Where(e => e.AuthUserId == userId).FirstOrDefault();

                    Ingresos ingreso = IngresoDto.ToUnMap(request.Ingreso);
                    ingreso.FechaDeIngreso = DateTime.Now;
                    ingreso.DetallesUsuarios = currentUserDetails;

                    Polizas poliza = db.Polizas.Where(p => p.Id == polizaId).FirstOrDefault();
                    if (poliza == null)
                        throw new Exception("No se encontró la póliza solicitada");

                    poliza.Ingresos.Add(ingreso);
                    Depositantes depositante = db.Depositantes.Where(a => a.Id == request.Depositante.Id).FirstOrDefault();
                    if(depositante == null)
                    {
                        depositante = DepositanteDto.ToUnMap(request.Depositante);
                        poliza.Afianzado.Depositantes.Add(depositante);
                    }

                    depositante.Ingresos.Add(ingreso);
                    int nChanges = db.SaveChanges();
                    transaction.Commit();
                }
            }
            return false;
        }

        private List<SearchPolizasResponse> FindByFilter(SearchPolozasRequest filterObject, string sortingField, string sorting = "asc")
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                var query = db.Polizas
                    .Select(e => new
                    {
                        Id = e.Id,
                        Cantidad = e.Cantidad,
                        Afianzado = new AfianzadoDto
                        {
                            Id = e.Afianzado.Id,
                            Nombre = e.Afianzado.Nombre,
                            ApellidoPaterno = e.Afianzado.ApellidoPaterno,
                            ApellidoMaterno = e.Afianzado.ApellidoMaterno,
                            FechaDeNacimiento = e.Afianzado.FechaDeNacimiento
                        },
                        Afianzadora = new AfianzadoraDto
                        {
                            Id = e.Afianzadoras.Id,
                            Nombre = e.Afianzadoras.Nombre
                        },
                        AfianzadoId = e.Afianzado.Id,
                        AfianzadoraId = e.AfianzadoraId,
                        AveriguacionPrevia = e.AveriguacionPrevia,
                        Descripcion = e.Descripcion,
                        FechaDeAlta = e.FechaDeAlta,
                        IngresosCount = e.Ingresos.Count,
                        TotalIngresos = (decimal?)(from ing in db.Ingresos where ing.PolizaId == e.Id select new { Cantidad = ing.Cantidad }).Sum(ing => ing.Cantidad)
                    });

                if (filterObject != null)
                {
                    // poliza = poliza.Where(e => e.Nombre.Contains("") || e.ApellidoMaterno.Contains("") || e.ApellidoPaterno.Contains(""));
                    query = query.Where(e => e.AveriguacionPrevia.Contains(""));
                    if (filterObject.AveriguacionPrevia != null)
                    {
                        query = query.Where(e => e.AveriguacionPrevia.Contains(filterObject.AveriguacionPrevia));
                    }
                    if (filterObject.Afianzado != null)
                    {
                        if (filterObject.Afianzado.Nombre != null)
                        {
                            query = query.Where(e =>
                                e.Afianzado.ApellidoPaterno.Contains(filterObject.Afianzado.Nombre)
                                || e.Afianzado.ApellidoMaterno.Contains(filterObject.Afianzado.Nombre)
                                || e.Afianzado.Nombre.Contains(filterObject.Afianzado.Nombre));
                        }
                    }
                    if (filterObject.Afianzadora != null)
                    {
                        if (filterObject.Afianzadora.Nombre != null)
                        {
                            query = query.Where(e => e.Afianzadora.Nombre.Contains(filterObject.Afianzadora.Nombre));
                        }
                    }
                    if (filterObject.TotalIngresos > 0)
                    {
                        query = query.Where(e => e.TotalIngresos >= filterObject.TotalIngresos);
                    }
                    if (filterObject.Cantidad > 0)
                    {
                        query = query.Where(e => e.Cantidad >= filterObject.Cantidad);
                    }
                }

                switch (sortingField)
                {
                    case "AveriguacionPrevia":
                    default:
                        if (sorting == "asc")
                        {
                            query = query.OrderBy(e => e.AveriguacionPrevia);
                        }
                        else
                        {
                            query = query.OrderByDescending(e => e.AveriguacionPrevia);
                        }
                        break;
                    case "Afianzado.Nombre":
                        if (sorting == "asc")
                        {
                            query = query.OrderBy(e => e.Afianzado.Nombre);
                        }
                        else
                        {
                            query = query.OrderByDescending(e => e.Afianzado.Nombre);
                        }
                        break;
                    case "Afianzadora.Nombre":
                        if (sorting == "asc")
                        {
                            query = query.OrderBy(e => e.Afianzadora.Nombre);
                        }
                        else
                        {
                            query = query.OrderByDescending(e => e.Afianzadora.Nombre);
                        }
                        break;
                    case "FechaDeAlta":
                        if (sorting == "asc")
                        {
                            query = query.OrderBy(e => e.FechaDeAlta);
                        }
                        else
                        {
                            query = query.OrderByDescending(e => e.FechaDeAlta);
                        }
                        break;
                    case "Cantidad":
                        if (sorting == "asc")
                        {
                            query = query.OrderBy(e => e.Cantidad);
                        }
                        else
                        {
                            query = query.OrderByDescending(e => e.Cantidad);
                        }
                        break;
                    case "TotalIngresos":
                        if (sorting == "asc")
                        {
                            query = query.OrderBy(e => e.TotalIngresos);
                        }
                        else
                        {
                            query = query.OrderByDescending(e => e.TotalIngresos);
                        }
                        break;
                }
                List<SearchPolizasResponse> listModel = query.ToList()
                    .Select(e => new SearchPolizasResponse
                    {
                        Id = e.Id,
                        Afianzado = e.Afianzado,
                        Afianzadora = e.Afianzadora,
                        AfianzadoId = e.Afianzado.Id,
                        AfianzadoraId = e.AfianzadoraId,
                        AveriguacionPrevia = e.AveriguacionPrevia,
                        Descripcion = e.Descripcion,
                        FechaDeAlta = e.FechaDeAlta,
                        IngresosCount = e.IngresosCount,
                        TotalIngresos = e.TotalIngresos.GetValueOrDefault(),
                        Cantidad = e.Cantidad.GetValueOrDefault()
                    }).ToList();
                return listModel;
            }
        }

        public bool ExisteAfianzado(AfianzadoDto afianzado, PGJSistemaPolizasEntities db = null)
        {
            return FindByAfianzado(afianzado, db) != null;
        }

        private Afianzados FindByAfianzado(AfianzadoDto afianzado, PGJSistemaPolizasEntities _db = null)
        {
            return CheckContext((db) => db.Afianzados.Where(e => e.Nombre == afianzado.Nombre
                && e.ApellidoMaterno == afianzado.ApellidoMaterno
                && e.ApellidoPaterno == afianzado.ApellidoPaterno).FirstOrDefault<Afianzados>(), _db);
        }
    }

    public class SearchPolozasRequest
    {
        public string AveriguacionPrevia { get; set; }
        public decimal? TotalIngresos { get; set; }
        public AfianzadoraDto Afianzadora { get; set; }
        public AfianzadoDto Afianzado { get; set; }
        public string FechaDeAlta { get; set; }
        public decimal? Cantidad { get; set; }
    }

    public class SearchPolizasResponse
    {
        public string AveriguacionPrevia { get; set; }
        public AfianzadoDto Afianzado { get; set; }
        public double? ingresos { get; set; }
        public AfianzadoraDto Afianzadora { get; set; }
        public DateTime FechaDeAlta { get; set; }
        public int Id { get; internal set; }
        public int AfianzadoId { get; internal set; }
        public int AfianzadoraId { get; internal set; }
        public string Descripcion { get; internal set; }
        public int IngresosCount { get; internal set; }
        public decimal? TotalIngresos { get; internal set; }
        public decimal? Cantidad { get; internal set; }
    }

    public class PolizasCreateRequest
    {
        public DepositanteDto depositante { get; set; }
        public AfianzadoDto afianzado { get; set; }
        public PolizaDto poliza { get; set; }
        public AfianzadoraDto afianzadora { get; set; }
    }

    public class SearchIngresoRequest
    {
        public decimal? Cantidad { set; get; }
        public string Depositante { set; get; }
        public string Descripcion { set; get; }

        private string _FechaDeIngreso;
        public string FechaDeIngreso
        {
            set
            {
                _FechaDeIngreso = value;
                if (!string.IsNullOrEmpty(value))
                {
                    string[] strDate = value.Split('/');
                    if (strDate.Length == 1)
                        this.Day = strDate[0];
                    if (strDate.Length == 2)
                        this.Month = strDate[1];
                    if (strDate.Length == 3)
                        this.Year = strDate[2];
                }
            }
            get
            {
                return _FechaDeIngreso;
            }
        }
        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
    }

    public class IngresoCreateRequest
    {
        public IngresoDto Ingreso { set; get; }
        public DepositanteDto Depositante { set; get; }
    }

    public class PolizaResponse
    {
        public PolizaDto Poliza { get; set; }
        public decimal? TotalIngresos { get; set; }
    }
}
