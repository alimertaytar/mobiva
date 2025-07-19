using dealer.mobiva.Helpers;
using Newtonsoft.Json;
using Objects.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace dealer.mobiva.Controllers
{
    public class ToDosController : DefaultController
    {
        public async Task<ActionResult> Index()
        {
            var apiService = new ApiService();
            var user = SessionManager.CurrentAppUser;

            var result = await apiService.GetAppUserToDos(user.Id); // AppUserId'ye göre çağırıyoruz

            if (result.Result)
            {
                return View(result.AppUserToDos);
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
                return View(new List<AppUserToDoViewModel>());
            }
        }

 

        public async Task<ActionResult> ToDoEdit(int id)
        {
            var model = new AppUserToDoViewModel();
            var apiService = new ApiService();

            // Kullanıcı listesi (dropdown için gerekli)
            var usersResult = await apiService.GetAppUsersByDealerId(SessionManager.CurrentDealer.Id);
            ViewBag.AppUsers = usersResult.AppUsers ?? new List<AppUserViewModel>();

            if (id > 0)
            {
                var result = await apiService.GetAppUserToDoById(id);
                if (result != null && result.Result && result.AppUserToDo != null)
                {
                    model = result.AppUserToDo;
                }
            }
            else
            {
                model.ActiveFlg = true; // ✅ Yeni kayıt ise default true
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ToDoEdit(AppUserToDoViewModel model)
        {
            var apiService = new ApiService();

            // Kullanıcı listesini tekrar ViewBag'a koyuyoruz, çünkü View dönerken lazım olacak
            var usersResult = await apiService.GetAppUsersByDealerId(SessionManager.CurrentDealer.Id);
            ViewBag.AppUsers = usersResult.AppUsers ?? new List<AppUserViewModel>();

            // CreateUserId, server tarafında otomatik atanmalı. Eğer sıfırsa veya gönderilmediyse burada atama yapabilirsin
            if (model.Id == 0)
            {
                model.CreateUserId = SessionManager.CurrentAppUser.Id; // Örnek: login olan kullanıcı ID'si
                model.CreateDate = DateTime.Now;
            }
            else
            {
                // Güncellemede CreateDate ve CreateUserId'yi değiştirmiyoruz (opsiyonel)
            }

            // Eğer IsDone işaretliyse DoneDate'yi otomatik atayabiliriz, değilse sıfırlayabiliriz
            if (model.IsDone)
            {
                if (model.DoneDate == default)
                {
                    model.DoneDate = DateTime.Now;
                }
            }
            else
            {
                model.DoneDate = null;
                model.DoneDetail = null;
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Lütfen tüm zorunlu alanları doldurun.";
                ViewBag.MessageType = "danger";
                return View(model);
            }
            string json = JsonConvert.SerializeObject(model);

            var result = await apiService.SaveAppUserToDo(model);

            if (result != null && result.Result)
            {
                ViewBag.Message = "ToDo başarıyla kaydedildi.";
                ViewBag.MessageType = "success";

                // Güncel model ile View'ı göster
                return View(model);
            }

            ModelState.AddModelError("", result?.Message ?? "Bir hata oluştu.");
            ViewBag.Message = result?.Message ?? "Bir hata oluştu.";
            ViewBag.MessageType = "danger";

            return View(model);
        }


    }
}