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
            try
            {
                var apiService = new ApiService();

                // AppUser kontrolü
                if (SessionManager.CurrentAppUser == null)
                {
                    if (!TryGetIdFromCookie("AppUserId", out int userId))
                    {
                        RedirectToLogout(filterContext);
                        return;
                    }

                    var userResult = Task.Run(() =>
                        apiService.GetAppUserById(userId).ConfigureAwait(false).GetAwaiter().GetResult()
                    );

                    if (userResult.Result?.Result == true && userResult.Result.AppUser != null)
                    {
                        SessionManager.CurrentAppUser = userResult.Result.AppUser;

                        var dealersResult = Task.Run(() =>
                            apiService.GetDealersByAppUserId(userId).ConfigureAwait(false).GetAwaiter().GetResult()
                        );

                        if (dealersResult.Result?.Result == true && dealersResult.Result.Dealers != null)
                            SessionManager.CurrentDealers = dealersResult.Result.Dealers;
                    }
                }

                // Dealer kontrolü
                if (SessionManager.CurrentDealer == null)
                {
                    if (!TryGetIdFromCookie("DealerId", out int dealerId))
                    {
                        RedirectToLogout(filterContext);
                        return;
                    }

                    var dealerResult = Task.Run(() =>
                        apiService.GetDealerById(dealerId).ConfigureAwait(false).GetAwaiter().GetResult()
                    );

                    if (dealerResult.Result?.Result == true && dealerResult.Result.Dealer != null)
                    {
                        SessionManager.CurrentDealer = dealerResult.Result.Dealer;
                    }
                }
            }
            catch (Exception ex)
            {
                // Hatalar gelişmiş log sistemine gönderilmeli
                System.Diagnostics.Debug.WriteLine($"[OnActionExecuting] Hata: {ex}");
                // Logout yapılmıyor — session boş kalabilir
            }

            base.OnActionExecuting(filterContext);
        }

        private bool TryGetIdFromCookie(string cookieName, out int id)
        {
            id = 0;
            var value = CookieManager.CookieGet(cookieName);
            return !string.IsNullOrEmpty(value) && int.TryParse(value, out id);
        }

        private void RedirectToLogout(ActionExecutingContext context)
        {
            context.Result = new RedirectResult("/Login/Logout");
        }


        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    var apiService = new ApiService();

        //    try
        //    {
        //        // 1. AppUser kontrolü
        //        if (SessionManager.CurrentAppUser == null)
        //        {
        //            var userIdStr = CookieManager.CookieGet("AppUserId");
        //            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
        //            {
        //                RedirectToLogout(filterContext);
        //                return;
        //            }

        //            // ConfigureAwait(false) ile deadlock riskini azaltıyoruz
        //            var userResult = Task.Run(() => apiService.GetAppUserById(userId).ConfigureAwait(false).GetAwaiter().GetResult()).Result;
        //            if (userResult?.Result == true && userResult.AppUser != null)
        //            {
        //                SessionManager.CurrentAppUser = userResult.AppUser;

        //                var dealersResult = Task.Run(() => apiService.GetDealersByAppUserId(userId).ConfigureAwait(false).GetAwaiter().GetResult()).Result;
        //                if (dealersResult?.Result == true && dealersResult.Dealers != null)
        //                    SessionManager.CurrentDealers = dealersResult.Dealers;
        //            }
        //        }

        //        // 2. Dealer kontrolü
        //        if (SessionManager.CurrentDealer == null)
        //        {
        //            var dealerIdStr = CookieManager.CookieGet("DealerId");
        //            if (string.IsNullOrEmpty(dealerIdStr) || !int.TryParse(dealerIdStr, out int dealerId))
        //            {
        //                RedirectToLogout(filterContext);
        //                return;
        //            }

        //            var dealerResult = Task.Run(() => apiService.GetDealerById(dealerId).ConfigureAwait(false).GetAwaiter().GetResult()).Result;
        //            if (dealerResult?.Result == true && dealerResult.Dealer != null)
        //            {
        //                SessionManager.CurrentDealer = dealerResult.Dealer;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Hata detayını logla, istersen kendi log sistemini entegre et
        //        System.Diagnostics.Debug.WriteLine($"OnActionExecuting hata: {ex}");
        //        // Burada logout yapma, sadece session boş kalır
        //    }

        //    base.OnActionExecuting(filterContext);
        //}

        //private void RedirectToLogout(ActionExecutingContext filterContext)
        //{
        //    filterContext.Result = new RedirectToRouteResult(
        //        new System.Web.Routing.RouteValueDictionary
        //        {
        //            { "controller", "Login" },
        //            { "action", "Logout" }
        //        });
        //}
    }
}
