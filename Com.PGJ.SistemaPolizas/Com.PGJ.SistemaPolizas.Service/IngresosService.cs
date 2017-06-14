using Com.PGJ.SistemaPolizas.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.PGJ.SistemaPolizas.Service
{
    public class IngresosService
    {
        public decimal GetTotalIngresos()
        {
            decimal total = 0;
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                decimal? d = (from i in db.Ingresos select (decimal?)i.Cantidad).Sum() ?? 0;
                total = d.GetValueOrDefault();
            }
            return total;
        }

        public decimal GetTotalIngresos(string anio)
        {
            decimal total = 0;
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                decimal? d = (from i in db.Ingresos where i.FechaDeIngreso.Year.ToString() == anio select (decimal?)i.Cantidad).Sum() ?? 0;
                total = d.GetValueOrDefault();
            }
            return total;
        }
    }
}
