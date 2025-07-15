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
        private static T Get<T>(string key) where T : class
        {
            try
            {
                return HttpContext.Current.Session[key] as T;
            }
            catch (Exception ex)
            {
                // İstersen loglama yapabilirsin, örn:
                // Logger.Log("Session get error: " + ex.Message);
                return null;
            }
        }
        private static void Set(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
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
