using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.PGJ.SistemaPolizas.Data.Model;
using PagedList;

namespace Com.PGJ.SistemaPolizas.Service
{
    public class AreasService
    {
        public Areas FindById(int areaId)
        {
            Areas area = null;
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                area = db.Areas.Where(e => e.Id == areaId).FirstOrDefault();
            }
            return area;
        }

        public IEnumerable<Areas> GetAllEnumerable()
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                return db.Areas.AsEnumerable();
            }
        }

        public Task<List<Areas>> GetAllAsync()
        {
            List<Areas> list = GetAll();
            return Task.FromResult(list);
        }

        public List<Areas> GetAll()
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                var list = db.Areas.ToList().Select(e => new
                Areas
                {
                    Id = e.Id,
                    Nombre = e.Nombre
                }).ToList();
                return list;
            }
        }

        public Task<IPagedList<Areas>> FindByFilterAsync(out int total, int page, int count, string sorting, string filter)
        {
            return Task.FromResult(FindByFilter(out total, page, count, sorting, filter));
        }

        public IPagedList<Areas> FindByFilter(out int total, int page = 1, int count = 10, string sorting = "asc", string filter = "")
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
                total = query.Count();
                var data = query.ToList().Select(e => new Areas
                {
                    Id = e.Id,
                    Nombre = e.Nombre
                }).ToPagedList(page, count);
                return data;
            }
        }
    }
}
