using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.PGJ.SistemaPolizas.Data.Model;

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

        public List<Areas> GetAll()
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                var list = db.Areas.ToList();
                return list;
            }
        }
    }
}
