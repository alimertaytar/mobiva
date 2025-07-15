using System.Web.Mvc;
using dealer.mobiva.Helpers;
using Objects.ViewModel;
using System.Threading.Tasks;
using System;

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

                    // ConfigureAwait(false) ile deadlock riskini azaltıyoruz
                    var userResult = Task.Run(() => apiService.GetAppUserById(userId).ConfigureAwait(false).GetAwaiter().GetResult()).Result;
                    if (userResult?.Result == true && userResult.AppUser != null)
                    {
                        SessionManager.CurrentAppUser = userResult.AppUser;

                        var dealersResult = Task.Run(() => apiService.GetDealersByAppUserId(userId).ConfigureAwait(false).GetAwaiter().GetResult()).Result;
                        if (dealersResult?.Result == true && dealersResult.Dealers != null)
                            SessionManager.CurrentDealers = dealersResult.Dealers;
                    }
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

                    var dealerResult = Task.Run(() => apiService.GetDealerById(dealerId).ConfigureAwait(false).GetAwaiter().GetResult()).Result;
                    if (dealerResult?.Result == true && dealerResult.Dealer != null)
                    {
                        SessionManager.CurrentDealer = dealerResult.Dealer;
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata detayını logla, istersen kendi log sistemini entegre et
                System.Diagnostics.Debug.WriteLine($"OnActionExecuting hata: {ex}");
                // Burada logout yapma, sadece session boş kalır
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
