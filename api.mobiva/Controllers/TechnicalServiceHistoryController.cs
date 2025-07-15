using Microsoft.AspNetCore.Mvc;
using Objects.ViewModel;
using Objects;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Objects.ApiModel;

namespace api.mobiva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnicalServiceHistoryController : ControllerBase
    {
        private readonly ContextApi _context;
        private readonly ContextApiHelper _helper;

        public TechnicalServiceHistoryController(ContextApi context)
        {
            _context = context;
            _helper = new ContextApiHelper(_context);
        }

        #region GetTechnicalServiceHistories
        [HttpPost("GetTechnicalServiceHistories")]
        public async Task<GetTechnicalServiceHistoriesParameterResult> GetTechnicalServiceHistories([FromBody] GetTechnicalServiceHistoriesParameter param)
        {
            var result = new GetTechnicalServiceHistoriesParameterResult();

            try
            {
                var filtered = await _helper.GetAllAsync<TechnicalServiceHistory>(
                    x => x.ActiveFlg && x.TechnicalServiceId == param.TechnicalServiceId
                );

                result.TechnicalServiceHistories = ObjectHelper.MapList<TechnicalServiceHistory, TechnicalServiceHistoryViewModel>(filtered);
                result.Result = true;
                result.Message = "Success";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
            }

            return result;
        }


        #endregion

        #region GetTechnicalServiceHistoryById
        [HttpPost("GetTechnicalServiceHistoryById")]
        public async Task<GetTechnicalServiceHistoryByIdParameterResult> GetTechnicalServiceHistoryById([FromBody] GetTechnicalServiceHistoryByIdParameter param)
        {
            var result = new GetTechnicalServiceHistoryByIdParameterResult();

            try
            {
                var entity = await _helper.GetByIdAsync<TechnicalServiceHistory>(param.Id);

                if (entity != null && entity.ActiveFlg)
                    result.TechnicalServiceHistory = ObjectHelper.Map<TechnicalServiceHistory, TechnicalServiceHistoryViewModel>(entity);

                result.Result = true;
                result.Message = "Success";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
            }

            return result;
        }


        #endregion

        #region SaveTechnicalServiceHistory
        [HttpPost("SaveTechnicalServiceHistory")]
        public async Task<SaveTechnicalServiceHistoryParameterResult> SaveTechnicalServiceHistory([FromBody] SaveTechnicalServiceHistoryParameter param)
        {
            var result = new SaveTechnicalServiceHistoryParameterResult();

            try
            {
                var entity = ObjectHelper.Map<TechnicalServiceHistoryViewModel, TechnicalServiceHistory>(param.TechnicalServiceHistory);
                var opResult = await _helper.SaveAsync(entity);

                result.Result = opResult.Success;
                result.Message = opResult.Message;
                result.Id = opResult.Data;
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
            }

            return result;
        }


        #endregion
    }
}
