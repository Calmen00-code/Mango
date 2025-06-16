using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDTO>? coupons = new();
            ResponseDTO? responseDTO = await _couponService.GetAllCouponAsync();

            if (responseDTO != null && responseDTO.IsSuccess)
            {
                coupons = JsonConvert.DeserializeObject<List<CouponDTO>>(Convert.ToString(responseDTO.Result));
            }
            else
            {
                TempData["error"] = responseDTO?.Message;
            }
            return View(coupons);
        }

        public async Task<IActionResult> CouponCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDTO coupon)
        {
            if (ModelState.IsValid)
            {
                ResponseDTO? responseDTO = await _couponService.CreateCouponsAsync(coupon);

                if (responseDTO != null && responseDTO.IsSuccess)
                {
                    TempData["success"] = "Coupon created successfully!";
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = responseDTO?.Message;
                }
            }

            return View(coupon);
        }

        public async Task<IActionResult> CouponDelete(int couponId)
        {
            ResponseDTO? responseDTO = await _couponService.GetCouponByIdAsync(couponId);

            if (responseDTO != null && responseDTO.IsSuccess)
            {
                CouponDTO? coupon = JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(responseDTO.Result));
                return View(coupon);
            }
            else
            {
                TempData["error"] = responseDTO?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDTO coupon)
        {
            ResponseDTO? responseDTO = await _couponService.DeleteCouponsAsync(coupon.CouponId);

            if (responseDTO != null && responseDTO.IsSuccess)
            {
                TempData["success"] = "Coupon deleted successfully!";
                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                TempData["error"] = responseDTO?.Message;
            }
            return NotFound(coupon);
        }
    }
}
