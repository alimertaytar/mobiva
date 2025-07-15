using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dealer.mobiva.Helpers
{
    public static class CookieManager
    {
        public static void CookieCreate(string cookieName, string value, int daysToExpire = 3)
        {
            if (string.IsNullOrEmpty(cookieName) || value == null) return;

            var cookie = new HttpCookie(cookieName, value)
            {
                Expires = DateTime.Now.AddDays(daysToExpire),
                HttpOnly = true,
                Secure = true
            };

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static string CookieGet(string cookieName)
        {
            if (string.IsNullOrEmpty(cookieName)) return null;

            var cookie = HttpContext.Current?.Request?.Cookies[cookieName];
            return cookie?.Value;
        }

        public static void CookieDelete(string cookieName)
        {
            if (string.IsNullOrEmpty(cookieName)) return;

            var expiredCookie = new HttpCookie(cookieName)
            {
                Expires = DateTime.Now.AddYears(-1),
                HttpOnly = true,
                Secure = true
            };

            HttpContext.Current.Response.Cookies.Add(expiredCookie);
        }
    }
}