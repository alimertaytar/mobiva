using Microsoft.AspNetCore.Mvc;
using Objects.ViewModel;
using Objects;
using System.Collections.Generic;
using System.Threading.Tasks;
using Objects.ApiModel;

namespace api.mobiva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinceController : ControllerBase
    {
        private readonly ContextApi _context;
        private readonly ContextApiHelper _helper;

        public ProvinceController(ContextApi context)
        {
            _context = context;
            _helper = new ContextApiHelper(_context);
        }

        #region GetProvinces
        [HttpPost("GetProvinces")]
        public async Task<GetProvincesParameterResult> GetProvinces()
        {
            var result = new GetProvincesParameterResult();

            try
            {
                var filtered = await _helper.GetAllAsync<Province>(x => x.ActiveFlg);
                result.Provinces = ObjectHelper.MapList<Province, ProvinceViewModel>(filtered);
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

        #region GetProvinceById
        [HttpPost("GetProvinceById")]
        public async Task<GetProvinceByIdParameterResult> GetProvinceById([FromBody] GetProvinceByIdParameter param)
        {
            var result = new GetProvinceByIdParameterResult();

            try
            {
                var entity = await _helper.GetByIdAsync<Province>(param.Id);

                if (entity != null && entity.ActiveFlg)
                    result.Province = ObjectHelper.Map<Province, ProvinceViewModel>(entity);

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

        #region SaveProvince
        [HttpPost("SaveProvince")]
        public async Task<SaveProvinceParameterResult> SaveProvince([FromBody] SaveProvinceParameter param)
        {
            var result = new SaveProvinceParameterResult();
            var entity = ObjectHelper.Map<ProvinceViewModel, Province>(param.Province);
            var opResult = await _helper.SaveAsync(entity);
            result.Result = opResult.Success;
            result.Message = opResult.Message;
            result.Id = opResult.Data;
            return result;
        }


        #endregion

    }
}
