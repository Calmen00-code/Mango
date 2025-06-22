using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDTO>? products = new();
            ResponseDTO? response = await _productService.GetAllProductAsync();

            if (response != null && response.IsSuccess)
            {
                products = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(products);
        }

        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDTO product)
        {
            if (ModelState.IsValid)
            {
                ResponseDTO? response = await _productService.CreateProductAsync(product);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product created successfully!";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }

            return View(product);
        }

        public async Task<IActionResult> ProductEdit(int productId)
        {
            ResponseDTO? response = await _productService.GetProductByIdAsync(productId);
            if (response != null && response.IsSuccess)
            {
                ProductDTO product = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response?.Result));
                return View(product);
            }

            TempData["error"] = response?.Message;
            return RedirectToAction(nameof(ProductIndex));
        }

        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDTO product)
        {
            if (ModelState.IsValid)
            {
                ResponseDTO? response = await _productService.UpdateProductAsync(product);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product updated successfully!";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }

            return View(product);
        }

        public async Task<IActionResult> ProductDelete(int productId)
        {
            ResponseDTO? response = await _productService.GetProductByIdAsync(productId);
            if (response != null && response.IsSuccess)
            {
                ProductDTO? product = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                return View(product);
            }

            TempData["error"] = response?.Message;
            return RedirectToAction(nameof(ProductIndex));
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDTO product)
        {
            ResponseDTO? response = await _productService.DeleteProductAsync(product.ProductId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product deleted successfully!";
                return RedirectToAction(nameof(ProductIndex));
            }
            
            TempData["error"] = response?.Message;
            return View(product);
        }
    }
}
