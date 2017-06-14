using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.PGJ.SistemaPolizas.Service.Dto;
using Com.PGJ.SistemaPolizas.Data.Model;

namespace Com.PGJ.SistemaPolizas.Service
{
    public class EgresosService
    {
        public EgresoDto FindById(int egresoId)
        {
            EgresoDto dto = null;
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                Egresos result = db.Egresos.Where(e => e.Id == egresoId).FirstOrDefault();
                dto = EgresoDto.ToMap(result);
            }
            return dto;
        }

        public IEnumerable<EgresoDto> GetAllEnumerable()
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                return EgresoDto.ToMap(db.Egresos.ToList());
            }
        }

        public Task<List<EgresoDto>> GetAllAsync()
        {
            List<EgresoDto> list = GetAll();
            return Task.FromResult(list);
        }

        public List<EgresoDto> GetAll()
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                List<Egresos> listModel = db.Egresos.ToList();
                return EgresoDto.ToMap(listModel);
            }
        }

        public Task<List<EgresoDto>> FindByFilterAsync(EgresoDto filterObject, string sortingField, string sorting = "asc")
        {
            return Task.FromResult(FindByFilter(filterObject, sortingField, sorting));
        }


        public EgresoDto Save(string userId, EgresoDto egresoDto)
        {
            if (egresoDto == null)
                throw new Exception("Es necesario un egreso");
            if (egresoDto.MinisteriosPublicos == null)
                throw new Exception("Es necesario un ministerio público");

            DetalleUsuarioDto usuarioDto = null;
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                DetallesUsuarios usuario = db.DetallesUsuarios.Where(u => u.AuthUserId == userId).FirstOrDefault();
                usuarioDto = DetalleUsuarioDto.ToMap(usuario);
                egresoDto.DetalleUsuarioId = usuarioDto.Id;
            }

            egresoDto.FechaDeEgreso = DateTime.Now;
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                MinisteriosPublicos ministerioModel = db.MinisteriosPublicos.Where(m => m.Id == egresoDto.MinisteriosPublicos.Id).FirstOrDefault();
                if (ministerioModel == null)
                    throw new Exception("No se encontró el ministerio público solicitado");
                Egresos egresoModel = EgresoDto.ToUnMap(egresoDto);
                egresoModel.MinisteriosPublicos = ministerioModel;
                db.Egresos.Add(egresoModel);
                int n = db.SaveChanges();
                if (n > 0)
                {
                    egresoDto.DetallesUsuarios = usuarioDto;
                    return egresoDto;
                }
                else
                    return null;
            }
        }

        public decimal GetTotalEgresos(string anio)
        {
            decimal total = 0;
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                decimal? d = (from i in db.Egresos where i.FechaDeEgreso.Year.ToString() == anio select (decimal?)i.Cantidad).Sum() ?? 0;
                total = d.GetValueOrDefault();
            }
            return total;
        }

        public decimal GetTotalEgresos()
        {
            decimal total = 0;
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                decimal? d = (from i in db.Egresos select (decimal?)i.Cantidad).Sum() ?? 0;
                total = d.GetValueOrDefault();
            }
            return total;
        }

        public EgresoDto Update(EgresoDto dto)
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                Egresos model = EgresoDto.ToUnMap(dto);
                db.Entry<Egresos>(model).State = System.Data.Entity.EntityState.Modified;
                int n = db.SaveChanges();
                if (n > 0)
                    return dto;
                else
                    return null;
            }
        }

        public bool Delete(int id)
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                Egresos entity = db.Egresos.Where(e => e.Id == id).FirstOrDefault();
                if (entity == null)
                    return false;
                db.Egresos.Remove(entity);
                int n = db.SaveChanges();
                return n > 0;
            }
        }

        private List<EgresoDto> FindByFilter(EgresoDto filterObject, string sortingField, string sorting = "asc")
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                var query = db.Egresos
                    .Select(e => new
                    {
                        Id = e.Id,
                        Cantidad = e.Cantidad,
                        Descripcion = e.Descripcion,
                        MinisteriosPublicos = e.MinisteriosPublicos == null ? null : new
                        {
                            Id = e.MinisteriosPublicos.Id,
                            Nombre = e.MinisteriosPublicos.Nombre,
                            Autoridad = e.MinisteriosPublicos.Autoridad == null ? null : new
                            {
                                Id = e.MinisteriosPublicos.Autoridad.Id,
                                Nombre = e.MinisteriosPublicos.Autoridad.Nombre
                            }
                        },
                        FechaDeEgreso = e.FechaDeEgreso
                    });
                /*
                if (filterObject != null && filterObject.Egreso != null)
                {
                    // poliza = poliza.Where(e => e.Nombre.Contains("") || e.ApellidoMaterno.Contains("") || e.ApellidoPaterno.Contains(""));
                    query = query.Where(e => e.Descripcion.ToLower().Contains(filterObject.Egreso.Descripcion.ToLower()));
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
                */
                List<EgresoDto> listModel = query.ToList()
                    .Select(e => new EgresoDto
                    {
                        Id = e.Id,
                        Cantidad = e.Cantidad,
                        Descripcion = e.Descripcion,
                        MinisteriosPublicos = e.MinisteriosPublicos == null ? null : new MinisterioPublicoDto
                        {
                            Id = e.MinisteriosPublicos.Id,
                            Nombre = e.MinisteriosPublicos.Nombre,
                            Autoridad = e.MinisteriosPublicos.Autoridad == null ? null : new AutoridadDto
                            {
                                Id = e.MinisteriosPublicos.Autoridad.Id,
                                Nombre = e.MinisteriosPublicos.Autoridad.Nombre
                            }
                        },
                        FechaDeEgreso = e.FechaDeEgreso
                    }).ToList();
                return listModel;
            }
        }

        public List<EgresoDto> FindByFilter(string sorting = "asc", string filter = "")
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                var query = db.Egresos.Where(e => e != null);
                if (!string.IsNullOrEmpty(filter))
                    query = query.Where(e => e.Cantidad.ToString().Contains(filter));

                switch (sorting)
                {
                    case "asc":
                        query = query.OrderBy(e => e.Cantidad);
                        break;
                    case "desc":
                        query = query.OrderByDescending(e => e.Cantidad);
                        break;
                }
                List<Egresos> listModel = query.ToList();
                return EgresoDto.ToMap(listModel);
            }
        }
    }

    public class SearchEgresosRequest
    {
        public EgresoDto Egreso { get; set; }
    }

    public class SearchEgresosResponse : EgresoDto
    {

    }

}