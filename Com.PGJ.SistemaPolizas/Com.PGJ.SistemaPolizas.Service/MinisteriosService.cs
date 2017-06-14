using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.PGJ.SistemaPolizas.Service.Dto;
using Com.PGJ.SistemaPolizas.Data.Model;

namespace Com.PGJ.SistemaPolizas.Service
{
    public class MinisteriosService
    {
        public MinisterioPublicoDto FindById(int ministerioId)
        {
            MinisterioPublicoDto dto = null;
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                MinisteriosPublicos result = db.MinisteriosPublicos.Where(e => e.Id == ministerioId).FirstOrDefault();
                dto = MinisterioPublicoDto.ToMap(result);
            }
            return dto;
        }

        public IEnumerable<MinisterioPublicoDto> GetAllEnumerable()
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                return MinisterioPublicoDto.ToMap(db.MinisteriosPublicos.ToList());
            }
        }

        public Task<List<MinisterioPublicoDto>> GetAllAsync()
        {
            List<MinisterioPublicoDto> list = GetAll();
            return Task.FromResult(list);
        }

        public List<MinisterioPublicoDto> GetAll()
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                List<MinisterioPublicoDto> listModel = db.MinisteriosPublicos
                    .Select(m => new MinisterioPublicoDto
                    {
                        Id = m.Id,
                        AutoridadId = m.AutoridadId,
                        Nombre = m.Nombre,
                        Autoridad = m.Autoridad == null ? null : new AutoridadDto
                        {
                            Id = m.Autoridad.Id,
                            Nombre = m.Autoridad.Nombre
                        }
                    }).ToList();
                return listModel;
            }
        }

        public Task<List<MinisterioPublicoDto>> FindByFilterAsync(MinisterioPublicoDto objFilterObject, string sortingField, string sorting)
        {
            return Task.FromResult(FindByFilter(objFilterObject, sortingField, sorting));
        }

        public MinisterioPublicoDto Save(string nombre)
        {
            MinisterioPublicoDto dtoResult = null;
            if (!exists(nombre))
            {
                dtoResult = _Save(nombre);
            }
            return dtoResult;
        }

        public MinisterioPublicoDto Save(MinisterioCreateModel createModel)
        {
            if (createModel.Ministerio == null)
                throw new Exception("Es necesario especificar un ministerio público");
            if (createModel.Autoridad == null)
                throw new Exception("Es necesario especificar a una autoridad");
            if (exists(createModel.Ministerio.Nombre))
                throw new Exception(string.Format("El ministerio publico {0} ya existe.", createModel.Ministerio.Nombre));

            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                Autoridades autoridad = db.Autoridades.Where(a => a.Nombre == createModel.Autoridad.Nombre).FirstOrDefault();
                if (autoridad == null)
                    throw new Exception(string.Format("La autoridad solicitada no existe {0} no existe.", createModel.Autoridad.Nombre));

                MinisteriosPublicos ministerio = new MinisteriosPublicos();
                ministerio.Nombre = createModel.Ministerio.Nombre;
                autoridad.MinisteriosPublicos.Add(ministerio);
                int nRows = db.SaveChanges();
                if (nRows > 0)
                    return MinisterioPublicoDto.ToMap(ministerio);
            }
            return null;
        }

        private MinisterioPublicoDto _Save(string nombre)
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                MinisteriosPublicos model = new MinisteriosPublicos();
                model.Nombre = nombre;
                db.MinisteriosPublicos.Add(model);
                int n = db.SaveChanges();
                if (n > 0)
                    return MinisterioPublicoDto.ToMap(model);
                else
                    return null;
            }
        }

        private bool exists(string nombre)
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                MinisteriosPublicos aexists = db.MinisteriosPublicos.Where(e => e.Nombre.ToLower() == nombre.ToLower()).FirstOrDefault();
                return aexists != null;
            }
        }

        public MinisterioPublicoDto Update(MinisterioPublicoDto dto)
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                MinisteriosPublicos model = MinisterioPublicoDto.ToUnMap(dto);
                model.AutoridadId = model.Autoridad.Id;
                db.Entry<MinisteriosPublicos>(model).State = System.Data.Entity.EntityState.Modified;
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
                MinisteriosPublicos entity = db.MinisteriosPublicos.Where(e => e.Id == id).FirstOrDefault();
                if (entity == null)
                    return false;
                db.MinisteriosPublicos.Remove(entity);
                int n = db.SaveChanges();
                return n > 0;
            }
        }

        public List<MinisterioPublicoDto> FindByFilter(MinisterioPublicoDto filterObject, string sortingField, string sorting = "asc")
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                var query = (from m in db.MinisteriosPublicos select m);

                if (filterObject != null)
                {
                    if (!string.IsNullOrEmpty(filterObject.Nombre))
                    {
                        query = query.Where(m => m.Nombre.ToLower().Contains(filterObject.Nombre.ToLower()));
                    }
                    if (filterObject.Autoridad != null && !string.IsNullOrEmpty(filterObject.Autoridad.Nombre))
                    {
                        query = query.Where(a => a.Autoridad.Nombre.ToLower().Contains(filterObject.Autoridad.Nombre.ToLower()));
                    }
                }

                switch (sortingField)
                {
                    case "Autoridad.Nombre":
                        if (sorting == "asc")
                            query = query.OrderBy(e => e.Autoridad.Nombre);
                        else
                            query = query.OrderByDescending(e => e.Autoridad.Nombre);
                        break;
                    case "Nombre":
                    default:
                        if (sorting == "asc")
                            query = query.OrderBy(e => e.Nombre);
                        else
                            query = query.OrderByDescending(e => e.Nombre);
                        break;
                }

                List<MinisterioPublicoDto> listModel = query.ToList().Select(m => new MinisterioPublicoDto
                {

                    Id = m.Id,
                    Nombre = m.Nombre,
                    AutoridadId = m.AutoridadId,
                    Autoridad = m.Autoridad == null ? null : new AutoridadDto
                    {
                        Id = m.Autoridad.Id,
                        Nombre = m.Autoridad.Nombre
                    }
                }).ToList();
                return listModel;
            }
        }
    }

    public class MinisterioCreateModel
    {
        public MinisterioPublicoDto Ministerio { get; set; }
        public AutoridadDto Autoridad { get; set; }
    }

    public class MinisterioSearchModel
    {
        public MinisterioPublicoDto Ministerio { get; set; }
        public AutoridadDto Autoridad { get; set; }
    }
}
