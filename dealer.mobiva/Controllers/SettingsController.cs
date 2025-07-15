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
        #region ProductBrand

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

        #region ProductModel

        public async Task<ActionResult> ProductModels(int? productBrandId)
        {
            var apiService = new ApiService();

            // Önce markaları getir dropdown için
            var brandResult = await apiService.GetProductBrands();
            ViewBag.ProductBrands = brandResult.ProductBrands ?? new List<ProductBrandViewModel>();

            // Eğer seçili bir marka yoksa, liste boş gelsin
            if (!productBrandId.HasValue || productBrandId.Value <= 0)
            {
                ViewBag.SelectedBrandId = 0;
                return View(new List<ProductModelViewModel>());
            }

            // Seçili marka varsa modelleri getir
            var result = await apiService.GetProductModels(productBrandId.Value);

            ViewBag.SelectedBrandId = productBrandId.Value;

            if (result.Result)
            {
                return View(result.ProductModels);
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
                return View(new List<ProductModelViewModel>());
            }
        }

        public async Task<ActionResult> ProductModelEdit(int id, int? productBrandId)
        {
            var apiService = new ApiService();

            var brandResult = await apiService.GetProductBrands();
            ViewBag.ProductBrands = brandResult.ProductBrands ?? new List<ProductBrandViewModel>();

            var model = new ProductModelViewModel();

            if (id > 0)
            {
                var result = await apiService.GetProductModelById(id);
                if (result != null && result.Result && result.ProductModel != null)
                    model = result.ProductModel;
            }
            else
            {
                // Yeni model için productBrandId parametresi gelmişse seçili yap
                if (productBrandId.HasValue && productBrandId > 0)
                    model.ProductBrandId = productBrandId.Value;
                else if (ViewBag.ProductBrands.Count == 1)
                    model.ProductBrandId = ViewBag.ProductBrands[0].Id;
            }

            ViewBag.SelectedBrandId = model.ProductBrandId;

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ProductModelEdit(ProductModelViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Lütfen tüm zorunlu alanları doldurun.";
                ViewBag.MessageType = "danger";

                // Dropdown için markaları yeniden yükle
                var brandResult = await new ApiService().GetProductBrands();
                ViewBag.ProductBrands = brandResult.ProductBrands ?? new List<ProductBrandViewModel>();

                return View(model);
            }

            var apiService = new ApiService();
            var result = await apiService.SaveProductModel(model);

            if (result != null && result.Result)
            {
                ViewBag.Message = "Model başarıyla kaydedildi.";
                ViewBag.MessageType = "success";

                // Kaydetmeden sonra da markaları göndermeye devam et
                var brandResult = await apiService.GetProductBrands();
                ViewBag.ProductBrands = brandResult.ProductBrands ?? new List<ProductBrandViewModel>();

                return View(model);
            }

            ModelState.AddModelError("", result?.Message ?? "Bir hata oluştu.");
            ViewBag.Message = result?.Message ?? "Bir hata oluştu.";
            ViewBag.MessageType = "danger";

            // Hata durumunda da markaları tekrar yükle
            var brandsOnError = await apiService.GetProductBrands();
            ViewBag.ProductBrands = brandsOnError.ProductBrands ?? new List<ProductBrandViewModel>();

            return View(model);
        }


        #endregion


    }
}
