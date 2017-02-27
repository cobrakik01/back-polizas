using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;


using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using Microsoft.Owin.Security;
using Microsoft.Owin;
using Com.PGJ.SistemaPolizas.Models;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Microsoft.AspNet.Identity;

namespace Com.PGJ.SistemaPolizas.Controllers.Api
{
    [Authorize]
    [RoutePrefix("api/security")]
    public class SecurityController : ApiController
    {

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public SecurityController()
        {
        }

        public SecurityController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return Request.GetOwinContext().Authentication;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? Request.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        [Route("user/info")]
        [HttpGet]
        public async Task<IHttpActionResult> UserInfo()
        {
            string uname = User.Identity.Name;
            ApplicationUser aUser = await UserManager.FindByNameAsync(uname);
            if (aUser == null)
                return NotFound();
            return Ok(new
            {
                UserId = aUser.Id,
                UserName = aUser.UserName
            });
        }

        [Route("islogin")]
        [HttpGet]
        public IHttpActionResult IsLogin()
        {
            return Ok(new
            {
                Login = User.Identity.IsAuthenticated
            });
        }

        [Route("user/details")]
        [HttpGet]
        public IHttpActionResult UserDetails(int userId)
        {

            return Ok(new
            {
                Login = User.Identity.IsAuthenticated
            });
        }
    }
}