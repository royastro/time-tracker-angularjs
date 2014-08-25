using System.Web.Http;
using TimeTracker.DataTransferObjects;
using TimeTracker.Services;

namespace TimeTracker.Web.Controllers
{
    public class SecurityController : ApiController
    {
        private readonly ISecurityService securityService;

        public SecurityController(ISecurityService securityService)
        {
            this.securityService = securityService;
        }

        [AllowAnonymous]
        [HttpGet]
        public bool IsLoggedIn()
        {
            return securityService.IsAuthenticated;
        }

        [Authorize]
        public void LogOff()
        {
            securityService.Logout();
        }

        [Authorize]
        public dtoUserProfile GetCurrentUser()
        {
            var dtoUserProfile = securityService.GetCurrentUser();
            return dtoUserProfile;
        }
    

        [AllowAnonymous]
        [HttpPost]
        public bool Login(dtoLogin login)
        {
            var isLoggedIn = securityService.Login(login.Username, login.Password);

            return isLoggedIn;
        }
    }
}
