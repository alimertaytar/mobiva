using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using dealer.mobiva.Helpers;
using Objects.ViewModel;

namespace dealer.mobiva.Controllers
{
    public class SettingsController : DefaultController
    {
        public async Task<ActionResult> ProductBrands()
        {

            var apiService = new ApiService();

            var result = await apiService.GetProductBrands(); // .Result kullandık sen async ise await yaparsın

            if (result.Result)
            {
                return View(result.ProductBrands);
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
                return View(new List<ProductBrandViewModel>());
            }

        }

        #region ProductBrandEdit

        // GET: /Settings/ProductBrandEdit/{id}
        public async Task<ActionResult> ProductBrandEdit(int id)
        {
            var model = new ProductBrandViewModel();
            var apiService = new ApiService();
            var result = await apiService.GetProductBrandById(id);

            if (result != null && result.Result && result.ProductBrand != null)
                model = result.ProductBrand;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ProductBrandEdit(ProductBrandViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Lütfen tüm zorunlu alanları doldurun.";
                ViewBag.MessageType = "danger";
                return View(model);
            }

            var apiService = new ApiService();
            var result = await apiService.SaveProductBrand(model);

            if (result != null && result.Result)
            {
                ViewBag.Message = "Marka başarıyla kaydedildi.";
                ViewBag.MessageType = "success";

                // Güncellenmiş veya kaydedilmiş modeli API'den tekrar çekmek istersen burada yapabilirsin
                // model = (await apiService.GetProductBrandById(model.Id))?.ProductBrand ?? model;

                return View(model);
            }

            ModelState.AddModelError("", result?.Message ?? "Bir hata oluştu.");
            ViewBag.Message = result?.Message ?? "Bir hata oluştu.";
            ViewBag.MessageType = "danger";
            return View(model);
        }


        #endregion



    }
}
