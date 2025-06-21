using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;

        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            RequestDTO request = new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = loginRequestDTO,
                Url = SD.AuthAPIBase + "/api/auth/login"
            };

            return await _baseService.SendAsync(request, withBearer: false);
        }

        public async Task<ResponseDTO?> RegisterAsync(RegistrationRequestDTO registrationRequestDTO)
        {
            RequestDTO request = new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = registrationRequestDTO,
                Url = SD.AuthAPIBase + "/api/auth/register"
            };

            return await _baseService.SendAsync(request, withBearer: false);
        }

        public async Task<ResponseDTO?> AssignRoleAsync(RegistrationRequestDTO registrationRequestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = registrationRequestDTO,
                Url = SD.AuthAPIBase + "/api/auth/AssignRole"
            });
        }
    }
}
