using BusinessLogicLayer.DiscountService;
using BusinessLogicLayer.OrderService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Authorize]
    public class DiscountController : Controller
    {
        private readonly DiscountLogic _discountLogic;
        private readonly OrderLogic _orderLogic;
        public DiscountController(OrderLogic orderLogic, DiscountLogic discountLogic)
        {
            _discountLogic = discountLogic;
            _orderLogic = orderLogic;
        }
        [HttpGet]
        public async Task<IActionResult> DiscountCheck(string code, int orderId)
        {
            var HaveDiscount = await _discountLogic.DiscountDetails(code);
            var result = _discountLogic.IsValid(HaveDiscount);
            var order =await  _orderLogic.OrderDetail(orderId);
            var message = "";
            decimal value = 0;
            if (result.result)
            {
                message = "کد تخفیف با موفقیت اعمال شد";
                order.DiscountValue = HaveDiscount.Value;
            }
            else
            {
                order.DiscountValue = 0;
                message = result.message;
            }
            if (_orderLogic.EditOrder(order))
            {
                value = HaveDiscount.Value;
            }
            else
            {
                value = 0;
            }
            return Json(new { result = result.result, text = message, discountValue = value });
        }
    }
}
