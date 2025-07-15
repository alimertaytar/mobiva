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
    public class CountyController : ControllerBase
    {
        private readonly ContextApi _context;
        private readonly ContextApiHelper _helper;

        public CountyController(ContextApi context)
        {
            _context = context;
            _helper = new ContextApiHelper(_context);
        }

        #region GetCounties
        [HttpPost("GetCounties")]
        public async Task<GetCountiesParameterResult> GetCounties([FromBody] GetCountiesParameter param)
        {
            var result = new GetCountiesParameterResult();

            try
            {
                var filtered = await _helper.GetAllAsync<County>(
                    x => x.ActiveFlg && x.ProvinceId == param.ProvinceId
                );

                result.Counties = ObjectHelper.MapList<County, CountyViewModel>(filtered);
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

        #region SaveCounty
        [HttpPost("SaveCounty")]
        public async Task<SaveCountyParameterResult> SaveCounty([FromBody] SaveCountyParameter param)
        {
            var result = new SaveCountyParameterResult();
            var entity = ObjectHelper.Map<CountyViewModel, County>(param.County);
            var opResult = await _helper.SaveAsync(entity);
            result.Result = opResult.Success;
            result.Message = opResult.Message;
            result.Id = opResult.Data;
            return result;
        }


        #endregion

        #region GetCountyById
        [HttpPost("GetCountyById")]
        public async Task<GetCountyByIdParameterResult> GetCountyById([FromBody] GetCountyByIdParameter param)
        {
            var result = new GetCountyByIdParameterResult();

            try
            {
                var entity = await _helper.GetByIdAsync<County>(param.Id);

                if (entity != null && entity.ActiveFlg)
                    result.County = ObjectHelper.Map<County, CountyViewModel>(entity);

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
