using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.PGJ.SistemaPolizas.Service.Dto;
using Com.PGJ.SistemaPolizas.Data.Model;

namespace Com.PGJ.SistemaPolizas.Service
{
    public class PolizasService
    {

        public PolizaDto Save(string userId, PolizaDto _polizaDto, DepositanteDto depositanteDto, AfianzadoDto afianzadoDto, AfianzadoraDto afianzadoraDto, decimal cantidad)
        {
            DateTime _createdAt = DateTime.Now;
            Polizas poliza = PolizaDto.ToUnMap(_polizaDto);
            Afianzados afianzado = AfianzadoDto.ToUnMap(afianzadoDto);
            Depositantes depositante = DepositanteDto.ToUnMap(depositanteDto);

            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                using (System.Data.Entity.DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    Afianzadoras afianzadora = db.Afianzadoras.Where(a => a.Id == afianzadoraDto.Id).FirstOrDefault();
                    DetallesUsuarios _currentUser = db.DetallesUsuarios.Where(e => e.AuthUserId == userId).FirstOrDefault();
                    
                    db.Afianzados.Add(afianzado);
                    /*
                    if (!this.ExisteDepositante(depositante, db))
                        afianzado.Depositantes.Add(depositante);
                    */
                    /*
                    poliza.AfianzadoraId = afianzadoraDto.Id;
                    poliza.Afianzado = afianzado;
                    */
                    poliza.FechaDeAlta = _createdAt;
                    //db.Polizas.Add(poliza);
                    //db.SaveChanges();

                    Ingresos ingreso = new Ingresos();
                    ingreso.Cantidad = cantidad;
                    ingreso.FechaDeIngreso = _createdAt;
                    ingreso.Descripcion = poliza.Descripcion;

                    afianzadora.Polizas.Add(poliza);
                    afianzado.Polizas.Add(poliza);
                    afianzado.Depositantes.Add(depositante);
                    poliza.Ingresos.Add(ingreso);
                    depositante.Ingresos.Add(ingreso);
                    _currentUser.Ingresos.Add(ingreso);

                    int n = db.SaveChanges();
                    transaction.Commit();
                    return _polizaDto;
                }
            }
        }

        private T CheckContext<T>(Func<PGJSistemaPolizasEntities, T> fn, PGJSistemaPolizasEntities db = null)
        {
            if (db == null)
                using (db = new PGJSistemaPolizasEntities())
                    return fn(db);
            else
                return fn(db);
        }

        private bool ExisteDepositante(Depositantes depositante, PGJSistemaPolizasEntities db = null)
        {
            return CheckContext((_db) => null != _db.Depositantes.Where(e => e.Nombre.Contains(depositante.Nombre) && e.ApellidoPaterno.Contains(depositante.ApellidoPaterno) && e.ApellidoMaterno.Contains(depositante.ApellidoMaterno)).FirstOrDefault(), db);
        }

        public bool ExisteAveriguacionPrevia(string averiguacionPrevia, PGJSistemaPolizasEntities _db = null)
        {
            return CheckContext((db) => null != db.Polizas.Where(e => e.AveriguacionPrevia == averiguacionPrevia).FirstOrDefault(), _db);
        }

        public bool ExisteAfianzado(AfianzadoDto afianzado, PGJSistemaPolizasEntities db = null)
        {
            return FindByAfianzado(afianzado, db) != null;
        }

        private Afianzados FindByAfianzado(AfianzadoDto afianzado, PGJSistemaPolizasEntities _db = null)
        {
            return CheckContext((db) => db.Afianzados.Where(e => e.Nombre == afianzado.Nombre
                && e.ApellidoMaterno == afianzado.ApellidoMaterno
                && e.ApellidoPaterno == afianzado.ApellidoPaterno).FirstOrDefault<Afianzados>(), _db);
        }
    }
}
