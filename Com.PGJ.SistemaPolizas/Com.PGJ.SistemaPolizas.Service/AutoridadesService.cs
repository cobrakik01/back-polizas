using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.PGJ.SistemaPolizas.Service.Dto;
using Com.PGJ.SistemaPolizas.Data.Model;

namespace Com.PGJ.SistemaPolizas.Service
{
    public class AutoridadesService
    {
        public Task<List<AutoridadDto>> GetAllAsync()
        {
            List<AutoridadDto> list = GetAll();
            return Task.FromResult(list);
        }

        public List<AutoridadDto> GetAll()
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                List<Autoridads> listModel = db.Autoridads.ToList();
                return AutoridadDto.ToMap(listModel);
            }
        }

        public Task<List<AutoridadDto>> FindByFilterAsync(string sorting, string filter)
        {
            return Task.FromResult(FindByFilter(sorting, filter));
        }

        public List<AutoridadDto> FindByFilter(string sorting = "asc", string filter = "")
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                var query = db.Autoridads.Where(e => e != null);
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
                List<Autoridads> listModel = query.ToList();
                return AutoridadDto.ToMap(listModel);
            }
        }

        public AutoridadDto Save(string nombre)
        {
            AutoridadDto dtoResult = null;
            if (!exists(nombre))
            {
                dtoResult = _Save(nombre);
            }
            return dtoResult;
        }

        private AutoridadDto _Save(string nombre)
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                Autoridads model = new Autoridads();
                model.Nombre = nombre;
                db.Autoridads.Add(model);
                int n = db.SaveChanges();
                if (n > 0)
                    return AutoridadDto.ToMap(model);
                else
                    return null;
            }
        }

        private bool exists(string nombre)
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                Autoridads aexists = db.Autoridads.Where(e => e.Nombre.Contains(nombre)).FirstOrDefault();
                return aexists != null;
            }
        }

        public AutoridadDto Update(AutoridadDto dto)
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                Autoridads model = AutoridadDto.ToUnMap(dto);
                db.Entry<Autoridads>(model).State = System.Data.Entity.EntityState.Modified;
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
                Autoridads model = db.Autoridads.Where(e => e.Id == id).FirstOrDefault();
                if (model == null)
                    return false;
                db.Autoridads.Remove(model);
                int n = db.SaveChanges();
                return n > 0;
            }
        }
    }
}
