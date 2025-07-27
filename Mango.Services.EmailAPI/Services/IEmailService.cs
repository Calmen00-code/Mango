using Mango.Services.EmailAPI.DTO;

namespace Mango.Services.EmailAPI.Service
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDTO cartDTO);
    }
}
