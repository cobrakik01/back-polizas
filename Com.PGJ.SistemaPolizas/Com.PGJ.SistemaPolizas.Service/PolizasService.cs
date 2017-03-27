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

        public PolizaDto Save(string userId, PolizaDto poliza, DepositanteDto depositanteDto, AfianzadoDto afianzadoDto, AfianzadoraDto afianzadora, decimal cantidad)
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                using (System.Data.Entity.DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    Afianzados afianzado = AfianzadoDto.ToUnMap(afianzadoDto);
                    Depositantes depositante = DepositanteDto.ToUnMap(depositante);
                    DetallesUsuarios _currentUser = db.DetallesUsuarios.Where(e => e.AuthUserId == userId).FirstOrDefault();

                    db.Afianzados.Add(afianzado);
                    if (!this.ExisteDepositante(depositante, db))
                        afianzado.Depositantes.Add(depositante);

                    poliza.Afianzado = afianzado;
                    poliza.AfianzadoraId = afianzadora.Id;
                    poliza.FechaDeAlta = DateTime.Now;
                    db.Polizas.Add(poliza);
                    db.SaveChanges();

                    Models.Ingreso ingreso = new Models.Ingreso();
                    ingreso.Cantidad = cantidad;
                    ingreso.FechaDeIngreso = DateTime.Now;
                    ingreso.DepositanteId = depositante.Id;
                    ingreso.Descripcion = poliza.Descripcion;

                    Models.DetallesUsuario currentUserDetails = db.DetallesUsuarios.Where(e => e.UserId == _currentUser.UserId).FirstOrDefault();
                    ingreso.DetalleUsuarioId = currentUserDetails.Id;
                    ingreso.PolizaId = poliza.Id;
                    db.Ingresos.Add(ingreso);
                    db.SaveChanges();
                    transaction.Commit();
                    return poliza;
                }
            }
        }

        private bool ExisteDepositante(Depositantes depositante, PGJSistemaPolizasEntities db = null)
        {
            throw new NotImplementedException();
        }

        public bool ExisteAveriguacionPrevia(string averiguacionPrevia, PGJSistemaPolizasEntities db = null)
        {
            throw new NotImplementedException();
        }

        public bool ExisteAfianzado(AfianzadoDto afianzado, PGJSistemaPolizasEntities db = null)
        {
            throw new NotImplementedException();
        }
    }
}
