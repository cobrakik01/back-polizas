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
using Com.PGJ.SistemaPolizas.Service;
using Com.PGJ.SistemaPolizas.Service.Dto;

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
        public IHttpActionResult UserDetails()
        {
            UsuariosService usuarioService = new UsuariosService();
            AreasService areasService = new AreasService();

            Data.Model.DetallesUsuarios userDetails = usuarioService.FindById(User.Identity.GetUserId());
            AreaDto area = null;
            if (userDetails != null)
            {
                area = areasService.FindById(userDetails.AreaId);
                var responseModel = new
                {
                    detalles = new
                    {
                        Id = userDetails.Id,
                        UserId = userDetails.AuthUserId,
                        Nombre = userDetails.Nombre,
                        ApellidoPaterno = userDetails.ApellidoPaterno,
                        ApellidoMaterno = userDetails.ApellidoMaterno,
                        NumeroDeEmpleado = userDetails.NumeroDeEmpleado,
                        FechaDeNacimiento = userDetails.FechaDeNacimiento,
                        CreatedAt = userDetails.CreatedAt,
                        UpdatedAt = userDetails.UpdatedAt
                    },
                    area = new
                    {
                        Id = area != null ? area.Id : 0,
                        Nombre = area != null ? area.Nombre : ""
                    }
                };
                return Ok(responseModel);
            }
            return Ok();
        }

        [Route("user/update-details")]
        [HttpPost]
        public IHttpActionResult UpdateUserDetails(UserSecurityModel model)
        {
            UsuariosService service = new UsuariosService();
            DetalleUsuarioDto detalles = model.detalles;
            AreaDto area = model.area;
            detalles.AreaId = area.Id;
            try
            {
                if (service.UpdateOrSave(User.Identity.GetUserId(), detalles))
                    return Json(new { Message = new { Type = "success", Title = "Correcto!", Message = "El perfil se actualizo correctamente." } });
                else
                    return Json(new { Message = new { Type = "warning", Title = "Cuidado!", Message = "no fue posible actualizar los detalles del usuario." } });
            }
            catch (Exception ex)
            {
                return Json(new { Message = new { Type = "warning", Title = "", Message = ex.Message } });
            }
        }

        [Route("user/change-password")]
        [HttpPost]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return Ok(new { Message = new { Type = "success", Title = "Correcto!", Message = "El password se actualizo correctamente." } });
            }
            return Ok(new { Message = new { Type = "warning", Title = "Cuidado!", Message = "No fue posible actualizar el password, por favor verifique su password actual." } });
        }
    }

    public class UserSecurityModel
    {
        public DetalleUsuarioDto detalles { get; set; }
        public AreaDto area { get; set; }
    }
}