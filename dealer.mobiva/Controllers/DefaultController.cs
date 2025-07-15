using System.Web.Mvc;
using dealer.mobiva.Helpers;
using Newtonsoft.Json;
using Objects.ViewModel;

namespace dealer.mobiva.Controllers
{
    public class DefaultController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var apiService = new ApiService();

            try
            {
                // 1. AppUser kontrolü
                if (SessionManager.CurrentAppUser == null)
                {
                    var userIdStr = CookieManager.CookieGet("AppUserId");
                    if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
                    {
                        RedirectToLogout(filterContext);
                        return;
                    }

                    var userResult = apiService.GetAppUserById(userId).GetAwaiter().GetResult();
                    if (userResult?.Result == true && userResult.AppUser != null)
                    {
                        SessionManager.CurrentAppUser = userResult.AppUser;

                        // Bayi listesi de doldurulsun
                        var dealersResult = apiService.GetDealersByAppUserId(userId).GetAwaiter().GetResult();
                        if (dealersResult?.Result == true && dealersResult.Dealers != null)
                            SessionManager.CurrentDealers = dealersResult.Dealers;
                    }
                    // API’den veri alınmazsa logout yok, sadece session boş kalır.
                }

                // 2. Dealer kontrolü
                if (SessionManager.CurrentDealer == null)
                {
                    var dealerIdStr = CookieManager.CookieGet("DealerId");
                    if (string.IsNullOrEmpty(dealerIdStr) || !int.TryParse(dealerIdStr, out int dealerId))
                    {
                        RedirectToLogout(filterContext);
                        return;
                    }

                    var dealerResult = apiService.GetDealerById(dealerId).GetAwaiter().GetResult();
                    if (dealerResult?.Result == true && dealerResult.Dealer != null)
                    {
                        SessionManager.CurrentDealer = dealerResult.Dealer;
                    }
                    // API’den veri alınmazsa logout yok.
                }
            }
            catch
            {
                // İstersen buraya log ekle, ama logout yapma
            }

            base.OnActionExecuting(filterContext);
        }

        private void RedirectToLogout(ActionExecutingContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                new System.Web.Routing.RouteValueDictionary
                {
            { "controller", "Login" },
            { "action", "Logout" }
                });
        }

    }
}
