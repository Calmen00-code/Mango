using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO loginRequestDTO = new LoginRequestDTO();
            return View(loginRequestDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequest)
        {
            ResponseDTO response = await _authService.LoginAsync(loginRequest);
            
            if (response != null && response.IsSuccess)
            {
                LoginResponseDTO loginResponse = 
                    JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Result));

                // setting up JWT
                await SignInUser(loginResponse);
                _tokenProvider.SetToken(loginResponse.Token);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("CustomError", response.Message);
                return View(loginRequest);
            }
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
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();

            return RedirectToAction("Index", "Home");
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

        private async Task SignInUser(LoginResponseDTO login)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(login.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));
            identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
