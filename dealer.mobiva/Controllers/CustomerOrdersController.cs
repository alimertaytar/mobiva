using dealer.mobiva.Helpers;
using Objects.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dealer.mobiva.Controllers
{
    public class CustomerOrdersController : DefaultController
    {
        public async Task<ActionResult> Index()
        {
            var apiService = new ApiService();
            var dealer = SessionManager.CurrentDealer;

            var result = await apiService.GetCustomerOrdersByDealerId(dealer.Id);

            if (result.Result)
            {
                return View(result.CustomerOrders);
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
                return View(new List<CustomerOrderViewModel>());
            }
        }
        public async Task<ActionResult> CustomerOrderEdit(int id)
        {
            var apiService = new ApiService();
            var model = new CustomerOrderViewModel();

            if (id > 0)
            {
                var result = await apiService.GetCustomerOrderById(id);
                if (result.Result)
                {
                    model = result.CustomerOrder;
                }
                else
                {
                    TempData["ErrorMessage"] = result.Message;
                }
            }

            var customerResult = await apiService.GetCustomersByDealerId(SessionManager.CurrentDealer.Id);
            if (customerResult.Result)
            {
                ViewBag.CustomerSelectList = new SelectList(customerResult.Customers.OrderBy(c => c.Name), "Id", "Name", model.CustomerId);
            }
            else
            {
                ViewBag.CustomerSelectList = new SelectList(Enumerable.Empty<SelectListItem>());
                TempData["ErrorMessage"] = customerResult.Message;
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CustomerOrderEdit(CustomerOrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Lütfen tüm zorunlu alanları doldurun.";
                ViewBag.MessageType = "danger";

                // Müşteri listesi tekrar yüklenmeli ki View'da selectbox çalışsın
                var customerResult = await new ApiService().GetCustomersByDealerId(SessionManager.CurrentDealer.Id);
                if (customerResult.Result)
                    ViewBag.CustomerSelectList = new SelectList(customerResult.Customers.OrderBy(c => c.Name), "Id", "Name", model.CustomerId);
                else
                    ViewBag.CustomerSelectList = new SelectList(Enumerable.Empty<SelectListItem>());

                return View(model);
            }

            var apiService = new ApiService();
            var user = SessionManager.CurrentAppUser;
            var dealer = SessionManager.CurrentDealer;

            model.DealerId = dealer.Id;
            model.CreateUserId = user.Id;

            if (model.Id == 0 && model.CreateDate == default)
                model.CreateDate = DateTime.Now;

            var result = await apiService.SaveCustomerOrder(model);

            if (result != null && result.Result)
            {
                ViewBag.Message = "Sipariş başarıyla kaydedildi.";
                ViewBag.MessageType = "success";

                // Eğer istersek kaydettikten sonra model güncellenebilir
                model.Id = result.Id;
                // Yine müşterileri yükle
                var customerResult = await apiService.GetCustomersByDealerId(dealer.Id);
                if (customerResult.Result)
                    ViewBag.CustomerSelectList = new SelectList(customerResult.Customers.OrderBy(c => c.Name), "Id", "Name", model.CustomerId);
                else
                    ViewBag.CustomerSelectList = new SelectList(Enumerable.Empty<SelectListItem>());

                return View(model);
            }

            ModelState.AddModelError("", result?.Message ?? "Bir hata oluştu.");
            ViewBag.Message = result?.Message ?? "Bir hata oluştu.";
            ViewBag.MessageType = "danger";

            // Müşteri listesini yine yükle
            var customerResultFail = await apiService.GetCustomersByDealerId(dealer.Id);
            if (customerResultFail.Result)
                ViewBag.CustomerSelectList = new SelectList(customerResultFail.Customers.OrderBy(c => c.Name), "Id", "Name", model.CustomerId);
            else
                ViewBag.CustomerSelectList = new SelectList(Enumerable.Empty<SelectListItem>());

            return View(model);
        }

    }
}
