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
    public class TechnicalServiceStatusController : ControllerBase
    {
        private readonly ContextApi _context;
        private readonly ContextApiHelper _helper;

        public TechnicalServiceStatusController(ContextApi context)
        {
            _context = context;
            _helper = new ContextApiHelper(_context);
        }

        #region GetTechnicalServiceStatuses
        [HttpPost("GetTechnicalServiceStatuses")]
        public async Task<GetTechnicalServiceStatusesParameterResult> GetTechnicalServiceStatuses()
        {
            var result = new GetTechnicalServiceStatusesParameterResult();

            try
            {
                var filtered = await _helper.GetAllAsync<TechnicalServiceStatus>(x => x.ActiveFlg);
                result.TechnicalServiceStatuses = ObjectHelper.MapList<TechnicalServiceStatus, TechnicalServiceStatusViewModel>(filtered);
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

        #region SaveTechnicalServiceStatus
        [HttpPost("SaveTechnicalServiceStatus")]
        public async Task<SaveTechnicalServiceStatusParameterResult> SaveTechnicalServiceStatus([FromBody] SaveTechnicalServiceStatusParameter param)
        {
            var result = new SaveTechnicalServiceStatusParameterResult();
            var entity = ObjectHelper.Map<TechnicalServiceStatusViewModel, TechnicalServiceStatus>(param.TechnicalServiceStatus);
            var opResult = await _helper.SaveAsync(entity);
            result.Result = opResult.Success;
            result.Message = opResult.Message;
            result.Id = opResult.Data;
            return result;
        }


        #endregion

        #region GetTechnicalServiceStatusById
        [HttpPost("GetTechnicalServiceStatusById")]
        public async Task<GetTechnicalServiceStatusByIdParameterResult> GetTechnicalServiceStatusById([FromBody] GetTechnicalServiceStatusByIdParameter param)
        {
            var result = new GetTechnicalServiceStatusByIdParameterResult();

            try
            {
                var entity = await _helper.GetByIdAsync<TechnicalServiceStatus>(param.Id);

                if (entity != null && entity.ActiveFlg)
                    result.TechnicalServiceStatus = ObjectHelper.Map<TechnicalServiceStatus, TechnicalServiceStatusViewModel>(entity);

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
