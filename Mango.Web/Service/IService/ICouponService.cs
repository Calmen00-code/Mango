using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface ICouponService
    {
        Task<ResponseDTO?> GetCoupon(string couponCode);
        Task<ResponseDTO?> GetAllCouponAsync();
        Task<ResponseDTO?> GetCouponByIdAsync(int id);
        Task<ResponseDTO?> CreateCouponsAsync(CouponDTO couponDTO);
        Task<ResponseDTO?> UpdateCouponsAsync(CouponDTO couponDTO);
        Task<ResponseDTO?> DeleteCouponsAsync(int id);
    }
}
