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

            if (!result.Result)
            {
                return View(result.ProductBrands);
            }

            TempData["ErrorMessage"] = result.Message;
            return View(new List<ProductBrandViewModel>());
        }

      
    }
}
