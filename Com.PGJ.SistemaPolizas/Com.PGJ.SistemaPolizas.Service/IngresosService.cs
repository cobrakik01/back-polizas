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
                // var TotalIngresos = db.Polizas.Sum(poliza => poliza.Ingresos != null ? poliza.Ingresos.Sum(ingreso => ingreso != null ? ingreso.Cantidad : 0) : 0);
                // var s = db.Ingresos.Where(e => e != null).DefaultIfEmpty().Sum(e => e.Cantidad);
                decimal? d = (from i in db.Ingresos select (decimal?)i.Cantidad).Sum() ?? 0;
                total = d.GetValueOrDefault();
            }
            return total;
        }
    }
}
