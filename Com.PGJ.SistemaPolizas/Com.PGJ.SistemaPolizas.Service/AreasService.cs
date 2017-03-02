using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.PGJ.SistemaPolizas.Service.Dto;
using Com.PGJ.SistemaPolizas.Data.Model;

namespace Com.PGJ.SistemaPolizas.Service
{
    public class AreasService
    {
        public AreaDto FindById(int areaId)
        {
            AreaDto area = null;
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                Areas result = db.Areas.Where(e => e.Id == areaId).FirstOrDefault();
                area = AreaDto.ToMap(result);
            }
            return area;
        }

        public IEnumerable<AreaDto> GetAllEnumerable()
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                return AreaDto.ToMap(db.Areas.ToList());
            }
        }

        public Task<List<AreaDto>> GetAllAsync()
        {
            List<AreaDto> list = GetAll();
            return Task.FromResult(list);
        }

        public List<AreaDto> GetAll()
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                List<Areas> listModel = db.Areas.ToList();
                return AreaDto.ToMap(listModel);
            }
        }

        public Task<List<AreaDto>> FindByFilterAsync(string sorting, string filter)
        {
            return Task.FromResult(FindByFilter(sorting, filter));
        }

        public AreaDto Save(string nombre)
        {
            AreaDto dtoResult = null;
            if (!exists(nombre))
            {
                dtoResult = _Save(nombre);
            }
            return dtoResult;
        }

        private AreaDto _Save(string nombre)
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                Areas model = new Areas();
                model.Nombre = nombre;
                db.Areas.Add(model);
                int n = db.SaveChanges();
                if (n > 0)
                    return AreaDto.ToMap(model);
                else
                    return null;
            }
        }

        private bool exists(string nombre)
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                Areas aexists = db.Areas.Where(e => e.Nombre.Contains(nombre)).FirstOrDefault();
                return aexists != null;
            }
        }

        public AreaDto Update(AreaDto area)
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                Areas model = AreaDto.ToUnMap(area);
                db.Entry<Areas>(model).State = System.Data.Entity.EntityState.Modified;
                int n = db.SaveChanges();
                if (n > 0)
                    return area;
                else
                    return null;
            }
        }

        public bool Delete(int id)
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                Areas area = db.Areas.Where(e => e.Id == id).FirstOrDefault();
                if (area == null)
                    return false;
                db.Areas.Remove(area);
                int n = db.SaveChanges();
                return n > 0;
            }
        }

        public List<AreaDto> FindByFilter(string sorting = "asc", string filter = "")
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                var query = db.Areas.Where(e => e != null);
                if (!string.IsNullOrEmpty(filter))
                    query = query.Where(e => e.Nombre.Contains(filter));

                switch (sorting)
                {
                    case "asc":
                        query = query.OrderBy(e => e.Nombre);
                        break;
                    case "desc":
                        query = query.OrderByDescending(e => e.Nombre);
                        break;
                }
                List<Areas> listModel = query.ToList();
                return AreaDto.ToMap(listModel);
            }
        }
    }
}
