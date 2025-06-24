using Mango.Services.ProductAPI.Utility;
using Mango.Services.ShoppingCartAPI.Models.DTO;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCartAPI.Service
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            // this can be any name, we are using "Product" here
            var client = _httpClientFactory.CreateClient("Product");

            HttpRequestMessage message = new();
            // configuring the header
            message.Headers.Add("Accept", "application/json");

            // configuring the URL and API type
            message.RequestUri = new Uri(SD.ProductAPIBase + "/api/product");
            message.Method = HttpMethod.Get;

            // trigger API calls and validate result
            HttpResponseMessage? apiResponse = await client.SendAsync(message);
            string? apiContent = await apiResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ResponseDTO>(apiContent);

            if (response != null && response.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDTO>>(Convert.ToString(response.Result));
            }

            return new List<ProductDTO>();
        }
    }
}
