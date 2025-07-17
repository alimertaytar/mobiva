using dealer.mobiva.Helpers;
using Objects.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace dealer.mobiva.Controllers
{
    public class PersonnelsController : DefaultController
    {
        public async Task<ActionResult> AppUsers()
        {
            var apiService = new ApiService();

            var dealer = SessionManager.CurrentDealer;

            var result = await apiService.GetAppUsersByDealerId(dealer.Id);

            if (result.Result)
            {
                return View(result.AppUsers);
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
                return View(new List<AppUserViewModel>());
            }
        }
        public async Task<ActionResult> AppUserEdit(int id)
        {
            var model = new AppUserViewModel();
            var apiService = new ApiService();

            // Kullanıcı tiplerini dropdown için getir
            var userTypesResult = await apiService.GetAppUserTypes();
            ViewBag.AppUserTypes = userTypesResult.AppUserTypes ?? new List<AppUserTypeViewModel>();

            if (id > 0)
            {
                var result = await apiService.GetAppUserById(id);
                if (result != null && result.Result && result.AppUser != null)
                    model = result.AppUser;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AppUserEdit(AppUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Lütfen tüm zorunlu alanları doldurun.";
                ViewBag.MessageType = "danger";

                // Kullanıcı tiplerini tekrar yükle
                var userTypesResult = await new ApiService().GetAppUserTypes();
                ViewBag.AppUserTypes = userTypesResult.AppUserTypes ?? new List<AppUserTypeViewModel>();

                return View(model);
            }

            var apiService = new ApiService();
            var result = await apiService.SaveAppUser(model);

            if (result != null && result.Result)
            {
                ViewBag.Message = "Kullanıcı başarıyla kaydedildi.";
                ViewBag.MessageType = "success";

                // Kullanıcı tipleri yine lazım olacak
                var userTypesResult = await apiService.GetAppUserTypes();
                ViewBag.AppUserTypes = userTypesResult.AppUserTypes ?? new List<AppUserTypeViewModel>();

                return View(model);
            }

            ModelState.AddModelError("", result?.Message ?? "Bir hata oluştu.");
            ViewBag.Message = result?.Message ?? "Bir hata oluştu.";
            ViewBag.MessageType = "danger";

            // Hata durumunda tekrar yükle
            var userTypesOnError = await apiService.GetAppUserTypes();
            ViewBag.AppUserTypes = userTypesOnError.AppUserTypes ?? new List<AppUserTypeViewModel>();

            return View(model);
        }


    }
}