using Mango.Web.Models.DTO;

namespace Mango.Web.Models
{
    public class CartDTO
    {
        public CartHeaderDTO CartHeader { get; set; }
        public IEnumerable<CartDetailsDTO> CartDetails { get; set; }
    }
}
