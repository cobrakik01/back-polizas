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
                List<MinisteriosPublicos> listModel = db.MinisteriosPublicos.ToList();
                return MinisterioPublicoDto.ToMap(listModel);
            }
        }

        public Task<List<MinisterioPublicoDto>> FindByFilterAsync(string sorting, string filter)
        {
            return Task.FromResult(FindByFilter(sorting, filter));
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
                MinisteriosPublicos aexists = db.MinisteriosPublicos.Where(e => e.Nombre.Contains(nombre)).FirstOrDefault();
                return aexists != null;
            }
        }

        public MinisterioPublicoDto Update(MinisterioPublicoDto dto)
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                MinisteriosPublicos model = MinisterioPublicoDto.ToUnMap(dto);
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

        public List<MinisterioPublicoDto> FindByFilter(string sorting = "asc", string filter = "")
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                var query = db.MinisteriosPublicos.Where(e => e != null);
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
                List<MinisteriosPublicos> listModel = query.ToList();
                return MinisterioPublicoDto.ToMap(listModel);
            }
        }
    }
}
