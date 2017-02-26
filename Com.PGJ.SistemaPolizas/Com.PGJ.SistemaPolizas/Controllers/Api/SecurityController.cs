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

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<IHttpActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    ApplicationUser aUser = await UserManager.FindByEmailAsync(model.Email);
                    return Ok(new
                    {
                        UserId = aUser.Id
                    });
                case SignInStatus.LockedOut:
                    return Ok("Lockout");
                case SignInStatus.RequiresVerification:
                    return Ok();
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return Ok(model);
            }
        }

        [HttpPost]
        [Route("logout")]
        public IHttpActionResult Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return Ok(new { Logout = User.Identity.IsAuthenticated });
        }

        [Route("user/info")]
        [HttpPost]
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
    }
}