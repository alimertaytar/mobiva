using Microsoft.AspNetCore.Mvc;
using Objects.ViewModel;
using Objects;
using System.Collections.Generic;
using System.Threading.Tasks;
using static api.mobiva.Controllers.ProductController;
using Objects.ApiModel;

namespace api.mobiva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserTypeController : ControllerBase
    {
        private readonly ContextApi _context;
        private readonly ContextApiHelper _helper;

        public AppUserTypeController(ContextApi context)
        {
            _context = context;
            _helper = new ContextApiHelper(_context);
        }

        #region GetAppUserTypes
        [HttpPost("GetAppUserTypes")]
        public async Task<GetAppUserTypesParameterResult> GetAppUserTypes()
        {
            var result = new GetAppUserTypesParameterResult();

            try
            {
                var filtered = await _helper.GetAllAsync<AppUserType>(x => x.ActiveFlg);
                result.AppUserTypes = ObjectHelper.MapList<AppUserType, AppUserTypeViewModel>(filtered);
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

        #region SaveAppUserType
        [HttpPost("SaveAppUserType")]
        public async Task<SaveAppUserTypeParameterResult> SaveAppUserType([FromBody] SaveAppUserTypeParameter param)
        {
            var result = new SaveAppUserTypeParameterResult();
            var entity = ObjectHelper.Map<AppUserTypeViewModel, AppUserType>(param.AppUserType);
            var opResult = await _helper.SaveAsync(entity);
            result.Result = opResult.Success;
            result.Message = opResult.Message;
            result.Id = opResult.Data;
            return result;
        }
        #endregion

        #region GetAppUserTypeById
        [HttpPost("GetAppUserTypeById")]
        public async Task<GetAppUserTypeByIdParameterResult> GetAppUserTypeById([FromBody] GetAppUserTypeByIdParameter param)
        {
            var result = new GetAppUserTypeByIdParameterResult();

            try
            {
                var entity = await _helper.GetByIdAsync<AppUserType>(param.Id);

                if (entity != null && entity.ActiveFlg)
                    result.AppUserType = ObjectHelper.Map<AppUserType, AppUserTypeViewModel>(entity);

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
