using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Microsoft.AspNet.Identity.Owin;
using Com.PGJ.SistemaPolizas.Models;
using System.Threading.Tasks;
using PagedList;

namespace Com.PGJ.SistemaPolizas.Controllers.Api
{
    [Authorize]
    [RoutePrefix("api/users")]
    public class UsuariosController : ApiController
    {

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public UsuariosController()
        {
        }

        public UsuariosController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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

        [HttpGet]
        [Route("roles")]
        public IHttpActionResult UserInRoles(string UserName)
        {
            var roles = System.Web.Security.Roles.GetRolesForUser(UserName);
            return Ok(roles);
        }

        [HttpGet]
        [Route("roles/all")]
        public IHttpActionResult AllRoles()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var entityRoles = context.Roles.ToList().Select(e => new { Id = e.Id, Name = e.Name }).ToList();
                //List<string> roles = new List<string>();
                //entityRoles.ForEach(e => roles.Add(e.Name));
                return Ok(entityRoles);
            }
        }

        [HttpGet]
        [Route("{userName}/exists")]
        public async Task<IHttpActionResult> ExistsUserName(string userName)
        {
            ApplicationUser user = await UserManager.FindByNameAsync(userName);
            bool b = user != null;
            return Ok(new { exists = b });
        }

        [HttpGet]
        [Route("{rolName}/rol-exists")]
        public IHttpActionResult ExistsRolName(string rolName)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var role = context.Roles.Where(e => e.Name == rolName).ToList().Select(e => new { Id = e.Id, Name = e.Name }).FirstOrDefault();
                bool b = role != null;
                return Ok(new { exists = b });
            }
        }

        [HttpPost]
        [Route("roles/{rolName}")]
        public IHttpActionResult NewRolName(string rolName)
        {
            Microsoft.AspNet.Identity.EntityFramework.IdentityRole existsRole = null;
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                existsRole = context.Roles.Where(e => e.Name == rolName).FirstOrDefault();
            }
            if (existsRole != null)
            {
                return Ok(new { Message = new { Type = "warning", Title = "Cuidado!", Message = "El nmbre del rol ya existe." } });
            }
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Microsoft.AspNet.Identity.EntityFramework.IdentityRole role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole(rolName);
                context.Roles.Add(role);
                int n = context.SaveChanges();
                if (n > 0)
                {
                    return Json(new { Message = new { Type = "success", Title = "Correcto!", Message = "El rol se dio de alta correctamente." } });
                }
                else
                {
                    return Ok(new { Message = new { Type = "warning", Title = "Cuidado!", Message = "No fue posible registrar el rol." } });
                }
            }
        }

        [HttpPost]
        [Route()]
        public async Task<IHttpActionResult> NewUser(CreateUserAcountViewModel userVm)
        {
            ApplicationUser user = new ApplicationUser();
            user.UserName = userVm.UserName;
            user.Email = userVm.UserName;
            var result = await UserManager.CreateAsync(user, "default");
            if (result.Succeeded)
            {
                var userAdded = await UserManager.FindByNameAsync(user.UserName);
                var rResult = await UserManager.AddToRolesAsync(userAdded.Id, userVm.Roles);
                if (rResult.Succeeded)
                {
                    return Ok(new { Message = new { Type = "success", Title = "Correcto!", Message = "El usuario se dio de alta correctamente." } });
                }
            }
            string msg = "No fue posible registrar al usuario.\n";
            foreach (string error in result.Errors)
            {
                msg += error;
            }
            return Ok(new { Message = new { Type = "warning", Title = "Cuidado!", Message = msg } });
        }

        [Route()]
        [HttpGet]
        public async Task<SearchResultViewModel> Search(int page = 1, int count = 10, string sorting = "asc", string filter = "")
        {
            SearchResultViewModel response = new SearchResultViewModel();
            List<ApplicationUser> list = await FindByFilterAsync(sorting, filter);
            response.total = list.Count();
            response.result = list.ToPagedList(page, count);
            return response;
        }

        public Task<List<ApplicationUser>> FindByFilterAsync(string sorting = "", string filter = "")
        {
            return Task.FromResult(FindByFilter(sorting, filter));
        }

        private List<ApplicationUser> FindByFilter(string sorting, string filter)
        {
            var query = UserManager.Users.Where(e => e != null);
            if (!string.IsNullOrEmpty(filter))
                query = query.Where(e => e.UserName.Contains(filter));

            switch (sorting)
            {
                case "asc":
                    query = query.OrderBy(e => e.UserName);
                    break;
                case "desc":
                    query = query.OrderByDescending(e => e.UserName);
                    break;
            }
            List<ApplicationUser> listModel = query.ToList();
            return listModel;
        }
    }

    public class CreateUserAcountViewModel
    {
        public string UserName { get; set; }
        public string[] Roles { get; set; }
    }
}