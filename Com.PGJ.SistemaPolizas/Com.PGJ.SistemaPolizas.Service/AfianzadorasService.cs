using Com.PGJ.SistemaPolizas.Data.Model;
using Com.PGJ.SistemaPolizas.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.PGJ.SistemaPolizas.Service
{
    public class AfianzadorasService
    {
        public Task<List<AfianzadoraDto>> GetAllAsync()
        {
            List<AfianzadoraDto> list = GetAll();
            return Task.FromResult(list);
        }

        public List<AfianzadoraDto> GetAll()
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                List<Afianzadoras> listModel = db.Afianzadoras.ToList();
                return AfianzadoraDto.ToMap(listModel);
            }
        }

        public Task<List<AfianzadoraDto>> FindByFilterAsync(string sorting = "", string filter = "")
        {
            return Task.FromResult(FindByFilter(sorting, filter));
        }

        private List<AfianzadoraDto> FindByFilter(string sorting, string filter)
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                var query = db.Afianzadoras.Where(e => e != null);
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
                List<Afianzadoras> listModel = query.ToList();
                return AfianzadoraDto.ToMap(listModel);
            }
        }

        public AfianzadoraDto Save(string nombre)
        {
            AfianzadoraDto dtoResult = null;
            if (!exists(nombre))
            {
                dtoResult = _Save(nombre);
            }
            return dtoResult;
        }

        private AfianzadoraDto _Save(string nombre)
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                Afianzadoras model = new Afianzadoras();
                model.Nombre = nombre;
                db.Afianzadoras.Add(model);
                int n = db.SaveChanges();
                if (n > 0)
                    return AfianzadoraDto.ToMap(model);
                else
                    return null;
            }
        }

        private bool exists(string nombre)
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                Afianzadoras aexists = db.Afianzadoras.Where(e => e.Nombre.Contains(nombre)).FirstOrDefault();
                return aexists != null;
            }
        }

        public AfianzadoraDto Update(AfianzadoraDto area)
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                Afianzadoras model = AfianzadoraDto.ToUnMap(area);
                db.Entry<Afianzadoras>(model).State = System.Data.Entity.EntityState.Modified;
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
                Afianzadoras model = db.Afianzadoras.Where(e => e.Id == id).FirstOrDefault();
                if (model == null)
                    return false;
                db.Afianzadoras.Remove(model);
                int n = db.SaveChanges();
                return n > 0;
            }
        }
    }
}
