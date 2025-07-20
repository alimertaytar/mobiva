using dealer.mobiva.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dealer.mobiva.Controllers
{
    public class CustomersController : DefaultController
    {
        public async Task<ActionResult> Index()
        {
            var apiService = new ApiService();
            var dealer = SessionManager.CurrentDealer;

            var result = await apiService.GetCustomersByDealerId(dealer.Id);

            if (result.Result)
            {
                return View(result.Customers);
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
                return View(new List<CustomerViewModel>());
            }
        }

        public async Task<ActionResult> CustomerEdit(int id = 0)
        {
            var model = new CustomerViewModel();
            var apiService = new ApiService();

            if (id > 0)
            {
                var result = await apiService.GetCustomerById(id);
                if (result != null && result.Result && result.Customer != null)
                    model = result.Customer;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CustomerEdit(CustomerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Lütfen tüm zorunlu alanları doldurun.";
                ViewBag.MessageType = "danger";
                return View(model);
            }

            var apiService = new ApiService();
            var result = await apiService.SaveCustomer(model);

            if (result != null && result.Result)
            {
                ViewBag.Message = "Müşteri başarıyla kaydedildi.";
                ViewBag.MessageType = "success";
                return View(model);
            }

            ModelState.AddModelError("", result?.Message ?? "Bir hata oluştu.");
            ViewBag.Message = result?.Message ?? "Bir hata oluştu.";
            ViewBag.MessageType = "danger";

            return View(model);
        }
    }
}
