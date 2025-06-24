using Mango.Services.ProductAPI.Utility;
using Mango.Services.ShoppingCartAPI.Models.DTO;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCartAPI.Service
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CouponDTO> GetCoupon(string couponCode)
        {
            var client = _httpClientFactory.CreateClient("Coupon");

            // configuring http header
            HttpRequestMessage message = new();
            message.Headers.Add("Accept", "application/json");

            // configuring the URL and API type
            message.RequestUri = new Uri(SD.CouponAPIBase + $"/api/coupon/GetByCode/{couponCode}");
            message.Method = HttpMethod.Get;

            // trigger API calls and validate result
            HttpResponseMessage? apiResponse = await client.SendAsync(message);
            string? apiContent = await apiResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ResponseDTO>(apiContent);

            if (response != null && response.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(response.Result));
            }

            return new CouponDTO();
        }
    }
}
