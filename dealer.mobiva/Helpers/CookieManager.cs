using System;
using System.Web;

namespace dealer.mobiva.Helpers
{
    public static class CookieManager
    {
        public static void CookieCreate(string cookieName, string value, int daysToExpire = 3)
        {
            if (string.IsNullOrEmpty(cookieName) || value == null)
                return;

            var context = HttpContext.Current;
            if (context == null || context.Response == null)
                return;

            var cookie = new HttpCookie(cookieName, value)
            {
                Expires = DateTime.Now.AddDays(daysToExpire),
                HttpOnly = true,
                Secure = true
            };

            context.Response.Cookies.Remove(cookieName); // varsa temizle
            context.Response.Cookies.Add(cookie);        // yeniden ekle
        }

        public static string CookieGet(string cookieName)
        {
            if (string.IsNullOrEmpty(cookieName))
                return null;

            var context = HttpContext.Current;
            if (context == null || context.Request == null)
                return null;

            var cookie = context.Request.Cookies[cookieName];
            return cookie?.Value;
        }

        public static void CookieDelete(string cookieName)
        {
            if (string.IsNullOrEmpty(cookieName))
                return;

            var context = HttpContext.Current;
            if (context == null || context.Response == null)
                return;

            var expiredCookie = new HttpCookie(cookieName)
            {
                Expires = DateTime.Now.AddYears(-1),
                HttpOnly = true,
                Secure = true
            };

            context.Response.Cookies.Remove(cookieName); // önce sil
            context.Response.Cookies.Add(expiredCookie); // sonra boş expire ile üzerine yaz
        }
    }
}
