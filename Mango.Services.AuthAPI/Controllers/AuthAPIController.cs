using Mango.Services.AuthAPI.DTO;
using Mango.Services.AuthAPI.Models.DTO;
using Mango.Services.AuthAPI.RabbitMQSender;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IRabbitMQAuthMessageSender _messageBus;
        private readonly IConfiguration _configuration;
        protected ResponseDTO _response;

        public AuthAPIController(IAuthService authService, IRabbitMQAuthMessageSender messageBus, IConfiguration configuration)
        {
            _authService = authService;
            _response = new();
            _messageBus = messageBus;
            _configuration = configuration;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO registrationModel)
        {
            var errorMessage = await _authService.Register(registrationModel);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }
            _messageBus.SendMessage(registrationModel.Email, _configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue"));

            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginModel)
        {
            var loginResponse = await _authService.Login(loginModel);

            if (loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Username or password is incorrect";
                return BadRequest(_response);
            }
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AddRole([FromBody] RegistrationRequestDTO registrationModel)
        {
            bool assignRoleSuccessful = await _authService.AssignRole(registrationModel.Email, registrationModel.Role.ToUpper());
            if (!assignRoleSuccessful)
            {
                _response.IsSuccess = false;
                _response.Message = "Failed to assign role";
                return BadRequest(_response);
            }

            _response.Result = assignRoleSuccessful;
            return Ok(_response);
        }

    }
}
