using AutoMapper;
using Mango.Services.CouponAPI.Models.DTO;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Utility;
using Mango.Services.ProductAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDTO _response;
        private IMapper _mapper;

        public ProductAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDTO();
        }

        [HttpGet]
        public ResponseDTO Get()
        {
            try
            {
                IEnumerable<Product> products = _db.Products.ToList();
                _response.Result = _mapper.Map<IEnumerable<ProductDTO>>(products);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDTO Get(int id)
        {
            try
            {
                Product product = _db.Products.FirstOrDefault(u => u.ProductId == id);
                _response.Result = _mapper.Map<ProductDTO>(product);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpGet]
        [Route("GetByProductName/{name}")]
        public ResponseDTO GetByProductName(string name)
        {
            try
            {
                Product product = _db.Products.FirstOrDefault(u => u.Name == name);
                _response.Result = _mapper.Map<ProductDTO>(product);
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPost]
        [Authorize(Roles = SD.ROLE_ADMIN)]
        public ResponseDTO Post([FromBody] ProductDTO productDTO)
        {
            try
            {
                Product product = _mapper.Map<Product>(productDTO);
                _db.Products.Add(product);
                _db.SaveChanges();

                _response.Result = _mapper.Map<ProductDTO>(product);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPut]
        [Authorize(Roles = SD.ROLE_ADMIN)]
        public ResponseDTO Put([FromBody] ProductDTO productDTO)
        {
            try
            {
                Product updateProduct = _db.Products.FirstOrDefault(u => u.ProductId == productDTO.ProductId);

                if (updateProduct == null)
                {
                    throw new Exception("Product not found");
                }

                _mapper.Map(productDTO, updateProduct);

                _db.Products.Update(updateProduct);
                _db.SaveChanges();

                _response.Result = _mapper.Map<ProductDTO>(updateProduct);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = SD.ROLE_ADMIN)]
        public ResponseDTO Delete(int id)
        {
            try
            {
                Product product = _db.Products.FirstOrDefault(u => u.ProductId == id);
                _db.Products.Remove(product);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }
    }
}
