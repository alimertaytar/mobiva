using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Objects.ApiModel;
using Objects.ViewModel;
using Objects;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace api.mobiva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly ContextApi _context;
        private readonly ContextApiHelper _helper;

        public AppUserController(ContextApi context)
        {
            _context = context;
            _helper = new ContextApiHelper(_context);
        }

        #region SaveAppUser
        [HttpPost("SaveAppUser")]
        public async Task<SaveAppUserParameterResult> SaveAppUser([FromBody] SaveAppUserParameter param)
        {
            var result = new SaveAppUserParameterResult();
            var entity = ObjectHelper.Map<AppUserViewModel, AppUser>(param.AppUser);
            var opResult = await _helper.SaveAsync(entity);
            result.Result = opResult.Success;
            result.Message = opResult.Message;
            result.Id = opResult.Data;
            return result;
        }


        #endregion

        #region GetAppUserById
        [HttpPost("GetAppUserById")]
        public async Task<GetAppUserByIdParameterResult> GetAppUserById([FromBody] GetAppUserByIdParameter param)
        {
            var result = new GetAppUserByIdParameterResult();

            try
            {
                var entity = await _helper.GetByIdAsync<AppUser>(param.Id);

                if (entity != null && entity.ActiveFlg)
                    result.AppUser = ObjectHelper.Map<AppUser, AppUserViewModel>(entity);

                result.Result = true;
                result.Message = "Success";
            }
            catch (System.Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
            }

            return result;
        }



        #endregion

        #region GetAppUserLogin
        [HttpPost("GetAppUserLogin")]
        public async Task<GetAppUserLoginParameterResult> GetAppUserById([FromBody] GetAppUserLoginParameter param)
        {
            var result = new GetAppUserLoginParameterResult();

            try
            {
                var user = await _helper.GetSingleAsync<AppUser>(x => x.Email == param.Email);
                if (user == null)
                {
                    result.Result = false;
                    result.Message = "Sistemde Kayıt Bulunamadı.";
                    return result;
                }

                if (!user.ActiveFlg)
                {
                    result.Result = false;
                    result.Message = "Bu kullanıcı aktif değil.";
                    return result;
                }

                if (user.Password != param.Password)
                {
                    result.Result = false;
                    result.Message = "Şifre hatalı.";
                    return result;
                }

                result.Result = true;
                result.Message = "Başarılı";
                result.Id = user.Id;
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
            }

            return result;
        }



        #endregion

        #region GetAppUsersbyDealerId
        [HttpPost("GetAppUsersbyDealerId")]
        public async Task<GetAppUsersbyDealerIdParameterResult> GetAppUsersbyDealerId([FromBody] GetAppUsersbyDealerIdParameter param)
        {
            var result = new GetAppUsersbyDealerIdParameterResult();

            try
            {
                var userTypes = await _helper.GetAllAsync<AppUserType>();

                var filtered = await _helper.GetAllAsync<AppUser>(
     x => x.ActiveFlg && x.DealerAppUser.Any(y => y.DealerId == param.DealerId)
 );

                result.AppUsers = filtered.Select(u => new AppUserViewModel
                {
                    Id = u.Id,
                    NameSurname = u.NameSurname,
                    Email = u.Email,
                    Password = u.Password,
                    AppUserTypeId = u.AppUserTypeId,
                    UserType = userTypes.FirstOrDefault(t => t.Id == u.AppUserTypeId)?.Name,
                    CreateDate = u.CreateDate,
                    ActiveFlg = u.ActiveFlg
                }).ToList();
                result.Result = true;
                result.Message = "Success";
            }
            catch (System.Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
            }

            return result;
        }


        #endregion
    }
}
