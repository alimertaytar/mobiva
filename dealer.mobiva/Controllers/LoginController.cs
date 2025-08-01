﻿using dealer.mobiva.ViewModels;
using dealer.mobiva.Helpers;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using Objects;
using System.Linq;
using Objects.ViewModel;

namespace dealer.mobiva.Controllers
{
    public class LoginController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var appUser = SessionManager.CurrentAppUser;
            var apiService = new ApiService();

            // Session boşsa cookie'den AppUserId ve DealerId kontrolü yapılır
            if (appUser == null)
            {
                var appUserIdStr = CookieManager.CookieGet("AppUserId");
                if (int.TryParse(appUserIdStr, out int appUserId))
                {
                    // API'den kullanıcı bilgisi çekilip session'a yazılır
                    var userResult = await apiService.GetAppUserById(appUserId);
                    if (userResult?.AppUser != null)
                        SessionManager.CurrentAppUser = userResult.AppUser;
                }
            }

            var dealer = SessionManager.CurrentDealer;

            if (dealer == null)
            {
                var dealerIdStr = CookieManager.CookieGet("DealerId");
                if (int.TryParse(dealerIdStr, out int dealerId))
                {
                    var dealerResult = await apiService.GetDealerById(dealerId);
                    if (dealerResult?.Dealer != null)
                        SessionManager.CurrentDealer = dealerResult.Dealer;
                }
            }

            // Yeniden session'lara eriş
            appUser = SessionManager.CurrentAppUser;
            dealer = SessionManager.CurrentDealer;

            // Kullanıcı ve bayi varsa Home’a yönlendir
            if (appUser != null)
            {
                if (dealer != null)
                    return RedirectToAction("Index", "Home");
                else
                    return RedirectToAction("DealerSelection", "Login");
            }

            // Hiçbiri yoksa login ekranı gösterilir
            return View(new LoginViewModel());
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var apiService = new ApiService();
                var getAppUserLoginResponse = await apiService.GetAppUserLogin(model.Email, model.Password);

                if (getAppUserLoginResponse == null || getAppUserLoginResponse.Result != true)
                {
                    model.Message = getAppUserLoginResponse?.Message ?? "Giriş başarısız.";
                    return View("Index",model);
                }
                else
                {
                    var getDealersByAppUserIdResponse = await apiService.GetDealersByAppUserId(getAppUserLoginResponse.Id);
                    var getAppUserByIdResponse = await apiService.GetAppUserById(getAppUserLoginResponse.Id);
                    if(getDealersByAppUserIdResponse.Dealers != null && getDealersByAppUserIdResponse.Dealers.Count == 1)
                    {
                        SessionManager.CurrentAppUser = getAppUserByIdResponse.AppUser;
                        SessionManager.CurrentDealers = getDealersByAppUserIdResponse.Dealers;
                        SessionManager.CurrentDealer = getDealersByAppUserIdResponse.Dealers.FirstOrDefault();
                        CookieManager.CookieCreate("AppUserId", getAppUserLoginResponse.Id.ToString());
                        CookieManager.CookieCreate("DealerId", getDealersByAppUserIdResponse.Dealers.FirstOrDefault().Id.ToString());
                        return RedirectToAction("Index", "Home");
                    }
                    else if (getDealersByAppUserIdResponse.Dealers != null && getDealersByAppUserIdResponse.Dealers.Count > 1)
                    {
                        SessionManager.CurrentAppUser = getAppUserByIdResponse.AppUser;
                        SessionManager.CurrentDealers = getDealersByAppUserIdResponse.Dealers;
                        CookieManager.CookieCreate("AppUserId", getAppUserLoginResponse.Id.ToString());
                        return RedirectToAction("DealerSelection", "Login");
                    }
                    else
                    {
                        model.Message = getAppUserLoginResponse?.Message ?? "Giriş başarısız.";
                        return View("Index", model);
                    }
                }
            }
            catch (Exception ex)
            {
                model.Message = "Sunucu hatası: " + ex.Message;
                return View("Index",model);
            }
        }



        public ActionResult DealerSelection()
        {
            var appUser = SessionManager.CurrentAppUser;
            var dealer = SessionManager.CurrentDealer;
            var dealers = SessionManager.CurrentDealers;

            if (appUser == null)
                return RedirectToAction("Index", "Login");  // Giriş yapılmamış

            if (dealer != null)
                return RedirectToAction("Index", "Home");  // Bayi zaten seçilmiş

            // Bayiler varsa seçim ekranı göster
            if (dealers == null || dealers.Count <= 1)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DealerSelection(int SelectedDealerId)
        {
            var dealers = SessionManager.CurrentDealers;
            var selectedDealer = dealers?.FirstOrDefault(d => d.Id == SelectedDealerId);

            if (selectedDealer == null)
                return RedirectToAction("DealerSelection");

            SessionManager.CurrentDealer = selectedDealer;
            CookieManager.CookieCreate("DealerId", selectedDealer.Id.ToString());

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();

            CookieManager.CookieDelete("AccessToken");
            CookieManager.CookieDelete("DealerId");
            CookieManager.CookieDelete("AppUserId");

            return RedirectToAction("Index", "Login");
        }
    }
}
