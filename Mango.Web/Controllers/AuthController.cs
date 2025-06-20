using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO loginRequestDTO = new LoginRequestDTO();
            return View(loginRequestDTO);
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem() { Text=SD.ROLE_ADMIN, Value=SD.ROLE_ADMIN },
                new SelectListItem() { Text=SD.ROLE_CUSTOMER, Value=SD.ROLE_CUSTOMER }
            };
            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }
    }
}
