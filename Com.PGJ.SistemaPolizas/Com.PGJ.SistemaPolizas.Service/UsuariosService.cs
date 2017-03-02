using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.PGJ.SistemaPolizas.Data.Model;
using Com.PGJ.SistemaPolizas.Service.Dto;

namespace Com.PGJ.SistemaPolizas.Service
{
    public class UsuariosService
    {
        public DetallesUsuarios FindById(string strId)
        {
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                DetallesUsuarios detalles = db.DetallesUsuarios.Where(e => e.AuthUserId == strId).FirstOrDefault();
                return detalles;
            }
        }

        public bool UpdateOrSave(string userId, DetalleUsuarioDto detallesDto)
        {
            DetallesUsuarios detallesResult = null;
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
                detallesResult = db.DetallesUsuarios.Where(e => e.AuthUserId == userId).FirstOrDefault();

            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                DetallesUsuarios detallesModel = DetalleUsuarioDto.ToUnMap(detallesDto);
                detallesModel.AuthUserId = userId;
                if (detallesResult == null)
                {
                    detallesModel.UpdatedAt = detallesModel.CreatedAt = DateTime.Now;
                    db.DetallesUsuarios.Add(detallesModel);
                }
                else
                {
                    detallesModel.UpdatedAt = DateTime.Now;
                    db.Entry<DetallesUsuarios>(detallesModel).State = System.Data.Entity.EntityState.Modified;
                }
                int n = db.SaveChanges();
                return n > 0;
            }
        }
    }
}
