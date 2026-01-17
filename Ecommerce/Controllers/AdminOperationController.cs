using Ecommerce.Constrants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.Controllers
{
    [Authorize(Roles =nameof(Roles.Admin))]
    public class AdminOperationController : Controller
    {
        private readonly IUserOrderRepo _userOrderRepo;

        public AdminOperationController(IUserOrderRepo userOrderRepo)
        {
            _userOrderRepo = userOrderRepo;
        }
        public async Task<IActionResult> AllOrders()
        {
            var orders = await _userOrderRepo.UserOrders(true);
            return View(orders);
        }
        public async Task<IActionResult> TogglePaymentStatus(int orderId )
        {
            try
            {
                await _userOrderRepo.TogglePaymentStatus(orderId);
            }
            catch (Exception ex)
            {
                
            }
            return RedirectToAction(nameof(AllOrders));
        }
        public async Task<IActionResult> UpdatePaymentStatus(int orderId)
        {
            var order = await _userOrderRepo.GetOrderById(orderId);
            if (order is null)
                throw new Exception($"Order with Id {orderId} not found");
            var orderStatusList = (
                await _userOrderRepo.GetOrderStatuses()).Select(orderStatus => {
                    return new SelectListItem
                    {
                        Value = orderStatus.Id.ToString(),
                        Text = orderStatus.StatusName,
                        Selected =
                        order.OrderStatusId == orderStatus.Id
                    };
                });
            var data = new UpdateOrderModelStatus
            {
                orderId = orderId,
                OrderStatusId = order.OrderStatusId,
                OrderStatusList = orderStatusList
            };
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePaymentStatus(UpdateOrderModelStatus data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    data.OrderStatusList = (
                        await _userOrderRepo.GetOrderStatuses()).Select(orderStatus =>
                        {
                            return new SelectListItem
                            {
                                Value = orderStatus.Id.ToString(), Text = orderStatus.StatusName, Selected = 
                                orderStatus.Id == data.OrderStatusId
                            };
                        });
                    return View(data);
                }
                await _userOrderRepo.ChangeOrderStatus(data);
                TempData["msg"] = "Data Updated SuccessFuly";
            }
            catch(Exception ex)
            {
                TempData["msg"] = "Something Went Wrong";
            }
            return RedirectToAction(nameof(UpdatePaymentStatus), new { orderId = data.orderId });
        }
    }
}
