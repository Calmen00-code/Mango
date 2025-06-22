using AutoMapper;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class ShoppingCartAPIController : ControllerBase
    {
        private ResponseDTO _response;
        private IMapper _mapper;
        private readonly AppDbContext _db;

        public ShoppingCartAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            this._response = new ResponseDTO();
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDTO> CartUpsert([FromBody] CartDTO cart)
        {
            try
            {
                var cartHeaderFromDb = await _db.CartHeaders.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserId == cart.CartHeader.UserId);
                
                if (cartHeaderFromDb == null)
                {
                    // create header and details if this is the first item for the user's cart
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cart.CartHeader);
                    _db.CartHeaders.Add(cartHeader);
                    await _db.SaveChangesAsync();

                    cart.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    _db.CartDetails.Add(_mapper.Map<CartDetails>(cart.CartDetails.First()));
                    await _db.SaveChangesAsync();
                }
                else
                {
                    // if header is existed, check if details has same product
                    var cartDetailsFromDb = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                        u => u.ProductId == cart.CartDetails.First().ProductId &&
                        u.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                    
                    if (cartDetailsFromDb == null)
                    {
                        // create cart details if it is different product
                        cart.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        _db.CartDetails.Add(_mapper.Map<CartDetails>(cart.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        // for the same product, update the count instead
                        cart.CartDetails.First().Count += cartDetailsFromDb.Count;
                        cart.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cart.CartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;

                        _db.CartDetails.Update(_mapper.Map<CartDetails>(cart.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                }
                _response.Result = cart;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }

            return _response;
        }
    }
}
