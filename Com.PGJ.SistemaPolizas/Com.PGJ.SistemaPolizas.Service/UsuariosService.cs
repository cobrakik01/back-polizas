﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.PGJ.SistemaPolizas.Data.Model;

namespace Com.PGJ.SistemaPolizas.Service
{
    public class UsuariosService
    {
        public DetallesUsuarios FindById(string strId)
        {
            DetallesUsuarios detalles = null;
            using (PGJSistemaPolizasEntities db = new PGJSistemaPolizasEntities())
            {
                detalles = db.DetallesUsuarios.Where(e => e.AuthUserId == strId).FirstOrDefault();
            }
            return detalles;
        }
    }
}
