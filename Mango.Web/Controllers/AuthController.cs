using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

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
            ViewBag.RoleList = BuildDisplayRoleList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            ResponseDTO result = await _authService.RegisterAsync(registrationRequestDTO);
            ResponseDTO assignRole;

            if (result != null && result.IsSuccess)
            {
                // at this point, user has been registered successfully

                // now check if user did not select any role
                // if so, default the role to Customer
                if (string.IsNullOrEmpty(registrationRequestDTO.Role))
                {
                    registrationRequestDTO.Role = SD.ROLE_CUSTOMER;
                }
                
                assignRole = await _authService.AssignRoleAsync(registrationRequestDTO);
                if (assignRole != null && assignRole.IsSuccess)
                {
                    // successfully assign role
                    TempData["success"] = "Registration successful";
                    return RedirectToAction(nameof(Login));
                }
            }

            ViewBag.RoleList = BuildDisplayRoleList();
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }


        // PRIVATE METHODS
        private List<SelectListItem> BuildDisplayRoleList()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem() { Text=SD.ROLE_ADMIN, Value = SD.ROLE_ADMIN },
                new SelectListItem() { Text=SD.ROLE_CUSTOMER, Value = SD.ROLE_CUSTOMER }
            };
            return roleList;
        }
    }
}
