using Objects;
using Objects.ApiModel;
using Objects.ViewModel;
using System;
using System.Collections.Generic;
using System.Web;

namespace dealer.mobiva.Helpers
{
    public static class SessionManager
    {
        public static T Get<T>(string key)
        {
            var context = HttpContext.Current;
            if (context == null || context.Session == null)
                return default(T);

            var obj = context.Session[key];
            if (obj == null)
                return default(T);

            return (T)obj;
        }

        private static void Set(string key, object value)
        {
            var context = HttpContext.Current;
            if (context == null || context.Session == null)
                return;

            context.Session[key] = value;
        }

        public static AppUserViewModel CurrentAppUser
        {
            get => Get<AppUserViewModel>("CurrentAppUser");
            set => Set("CurrentAppUser", value);
        }

        public static DealerViewModel CurrentDealer
        {
            get => Get<DealerViewModel>("CurrentDealer");
            set => Set("CurrentDealer", value);
        }

        public static List<DealerViewModel> CurrentDealers
        {
            get => Get<List<DealerViewModel>>("CurrentDealers");
            set => Set("CurrentDealers", value);
        }

        public static string AccessToken
        {
            get => Get<string>("AccessToken");
            set => Set("AccessToken", value);
        }
    }
}
