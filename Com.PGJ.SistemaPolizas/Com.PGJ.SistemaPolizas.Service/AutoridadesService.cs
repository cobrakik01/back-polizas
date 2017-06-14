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
                List<Autoridades> listModel = db.Autoridades.ToList();
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
                var query = db.Autoridades.Where(e => e != null);
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
                List<Autoridades> listModel = query.ToList();
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
                Autoridades model = new Autoridades();
                model.Nombre = nombre;
                db.Autoridades.Add(model);
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
                Autoridades aexists = db.Autoridades.Where(e => e.Nombre.Contains(nombre)).FirstOrDefault();
                return aexists != null;
            }
        }

        public AutoridadDto Update(AutoridadDto dto)
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                Autoridades model = AutoridadDto.ToUnMap(dto);
                db.Entry<Autoridades>(model).State = System.Data.Entity.EntityState.Modified;
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
                Autoridades model = db.Autoridades.Where(e => e.Id == id).FirstOrDefault();
                if (model == null)
                    return false;
                db.Autoridades.Remove(model);
                int n = db.SaveChanges();
                return n > 0;
            }
        }
    }
}
