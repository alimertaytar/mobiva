using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Objects;
using Objects.ApiModel;
using Objects.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace api.mobiva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ContextApi _context;
        private readonly ContextApiHelper _helper;
        public LoginController(ContextApi context)
        {
            _context = context;
            _helper = new ContextApiHelper(_context);
        }


        //#region Login

        //[HttpPost("Login")]
        //public async Task<LoginParameterResult> Login([FromBody] LoginParameter body)
        //{
        //    var result = new LoginParameterResult();

        //    try
        //    {
        //        var user = await _helper.GetSingleAsync<AppUser>(x => x.Email == body.Email);


        //        if (user == null)
        //        {
        //            result.Result = false;
        //            result.Message = "Sistemde Kayıt Bulunamadı.";
        //            return result;
        //        }

        //        if (!user.ActiveFlg)
        //        {
        //            result.Result = false;
        //            result.Message = "Bu kullanıcı aktif değil.";
        //            return result;
        //        }

        //        if (user.Password != body.Password)
        //        {
        //            result.Result = false;
        //            result.Message = "Şifre hatalı.";
        //            return result;
        //        }

        //        result.Result = true;
        //        result.Message = "Başarılı";
        //        result.AppUser = ObjectHelper.Map<AppUser, AppUserViewModel>(user);

        //        var dealerIds = _context.DealerAppUser
        //            .Where(x => x.AppUserId == user.Id)
        //            .Select(x => x.DealerId);

        //        var dealers = await _context.Dealer
        //            .Where(x => dealerIds.Contains(x.Id))
        //            .ToListAsync();

        //        result.Dealers = ObjectHelper.MapList<Dealer, DealerViewModel>(dealers);
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Result = false;
        //        result.Message = ex.Message;
        //    }

        //    return result;
        //}

     

        //#endregion


        //#region Login
        ////[Authorize(AuthenticationSchemes = "Bearer")]
        //[HttpPost]
        //public LoginParameterResult Login([FromBody] LoginParameter body)
        //{
        //    var result = new LoginParameterResult();
        //    var user = _context.AppUser.FirstOrDefault(x => x.Email == body.Email);
        //    if (user != null)
        //    {
        //        if (!user.ActiveFlg)
        //        {
        //            result.Result = false;
        //            result.Message = "Bu kullanıcı aktif değil.";
        //        }
        //        else if (user.Password == body.Password)
        //        {
        //            result.Result = true;
        //            result.Message = "Başarılı";
        //            result.AppUser = ObjectHelper.MapObject<AppUser, AppUserViewModel>(user);
        //            var dealerIds = _context.DealerAppUser.Where(x => x.AppUserId == user.Id).Select(x => x.DealerId);
        //            var dealers = _context.Dealer.Where(x => dealerIds.Contains(x.Id)).ToList();
        //            result.Dealers = ObjectHelper.MapList<Dealer, DealerViewModel>(dealers);

        //        }
        //        else
        //        {
        //            result.Result = false;
        //            result.Message = "Şifre hatalı.";
        //        }
        //    }
        //    else
        //    {
        //        result.Result = false;
        //        result.Message = "Sistemde Kayıt Bulunamadı.";
        //    }

        //    return result;
        //}
        //public class LoginParameter
        //{
        //    public string Email { get; set; }
        //    public string Password { get; set; }
        //}
        //public class LoginParameterResult
        //{
        //    public AppUserViewModel AppUser { get; set; }
        //    public List<DealerViewModel> Dealers { get; set; }
        //    public bool Result { get; set; }
        //    public string Message { get; set; }
        //}

        //#endregion


    }

}
