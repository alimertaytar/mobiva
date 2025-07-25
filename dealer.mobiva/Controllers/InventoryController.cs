using dealer.mobiva.Helpers;
using Objects;
using Objects.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace dealer.mobiva.Controllers
{
    public class InventoryController : DefaultController
    {
        // GET: Inventory
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ProductEdit(int id = 0, int productTypeId = 0)
        {
            var model = new ProductViewModel();
            var apiService = new ApiService();
            var appUser = SessionManager.CurrentAppUser;
            if (id > 0)
            {
                var result = await apiService.GetProductById(id);
                if (result != null && result.Result && result.Product != null)
                    model = result.Product;

                var productModelsResult = await apiService.GetProductModels(result.Product.ProductDetail.ProductBrandId);
                ViewBag.ProductModelList = productModelsResult?.ProductModels?
                    .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
                    .ToList() ?? new List<SelectListItem>();
            }
            else
            {
                // Yeni kayıt durumunda URL'den gelen ProductTypeId set edilsin
                model.ProductTypeId = productTypeId;
                model.ProductDetail = new ProductDetailViewModel();
                ViewBag.ProductModelList = new List<SelectListItem>();
            }

            // DropDown'lar
            var productTypesResult = await apiService.GetProductTypes();
            ViewBag.ProductTypesList = productTypesResult?.ProductTypes?
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
                .ToList() ?? new List<SelectListItem>();

            var productTypeSubsResult = await apiService.GetProductTypeSubs();
            ViewBag.ProductTypeSubsList = productTypeSubsResult?.ProductTypeSubs?
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
                .ToList() ?? new List<SelectListItem>();

            var productStatusesResult = await apiService.GetProductStatuses();
            ViewBag.ProductStatusList = productStatusesResult?.ProductStatuses?
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
                .ToList() ?? new List<SelectListItem>();

            var dealersResult = await apiService.GetDealersByAppUserId(appUser.Id);
            ViewBag.DealersList = dealersResult?.Dealers?
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
                .ToList() ?? new List<SelectListItem>();


            var productBrandsResult = await apiService.GetProductBrands();
            ViewBag.ProductBrandList = productBrandsResult?.ProductBrands?
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
                .ToList() ?? new List<SelectListItem>();

            model.DealerId = SessionManager.CurrentDealer?.Id ?? 0;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ProductEdit(ProductViewModel model)
        {
            var apiService = new ApiService();
            var appUser = SessionManager.CurrentAppUser;
            // DropDown'lar
            var productTypesResult = await apiService.GetProductTypes();
            ViewBag.ProductTypesList = productTypesResult?.ProductTypes?
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
                .ToList() ?? new List<SelectListItem>();

            var productTypeSubsResult = await apiService.GetProductTypeSubs();
            ViewBag.ProductTypeSubsList = productTypeSubsResult?.ProductTypeSubs?
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
                .ToList() ?? new List<SelectListItem>();

            var productStatusesResult = await apiService.GetProductStatuses();
            ViewBag.ProductStatusList = productStatusesResult?.ProductStatuses?
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
                .ToList() ?? new List<SelectListItem>();

            var dealersResult = await apiService.GetDealersByAppUserId(appUser.Id);
            ViewBag.DealersList = dealersResult?.Dealers?
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
                .ToList() ?? new List<SelectListItem>();


            var productBrandsResult = await apiService.GetProductBrands();
            ViewBag.ProductBrandList = productBrandsResult?.ProductBrands?
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
                .ToList() ?? new List<SelectListItem>();

            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Lütfen tüm zorunlu alanları doldurun.";
                ViewBag.MessageType = "danger";

               

                return View(model);
            }


            // DealerId'yi session'dan çek

            var result = await apiService.SaveProduct(model);

            var productModelsResult = await apiService.GetProductModels(model.ProductDetail.ProductBrandId);
            ViewBag.ProductModelList = productModelsResult?.ProductModels?
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
                .ToList() ?? new List<SelectListItem>();


            if (result != null && result.Result)
            {
                ViewBag.Message = "Ürün başarıyla kaydedildi.";
                ViewBag.MessageType = "success";

                

                return View(model);
            }

            ModelState.AddModelError("", result?.Message ?? "Bir hata oluştu.");
            ViewBag.Message = result?.Message ?? "Bir hata oluştu.";
            ViewBag.MessageType = "danger";

          
            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> GetProductModelById(int brandId)
        {
            var apiService = new ApiService();
            var result = await apiService.GetProductModels(brandId);

            var models = result?.ProductModels?
    .Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() })
    .ToList() ?? new List<SelectListItem>();

            return Json(models, JsonRequestBehavior.AllowGet);
        }
    }
}