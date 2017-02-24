using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Com.PGJ.SistemaPolizas.Controllers.Api
{
    [RoutePrefix("api/security")]
    public class SecurityController : ApiController
    {
        [Route("login")]
        //[HttpPost]
        [HttpGet]
        public IEnumerable<Models.ApplicationUser> Login()
        {
            return Models.ApplicationDbContext.Create().Users.AsEnumerable();
            //return new string[] { "Este es", "el login" };
        }
    }
}