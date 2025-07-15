using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Objects.ViewModel;
using Objects;
using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Objects.ApiModel;

namespace api.mobiva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnicalServiceController : ControllerBase
    {
        private readonly ContextApi _context;
        private readonly ContextApiHelper _helper;

        public TechnicalServiceController(ContextApi context)
        {
            _context = context;
            _helper = new ContextApiHelper(_context);
        }

        #region GetTechnicalServices
        [HttpPost("GetTechnicalServices")]
        public async Task<GetTechnicalServicesParameterResult> GetTechnicalServices([FromBody] GetTechnicalServicesParameter body)
        {
            var result = new GetTechnicalServicesParameterResult();

            try
            {
                var filtered = await _helper.GetAllAsync<TechnicalService>(x => x.ActiveFlg && x.DealerId == body.DealerId);
                result.TechnicalServices = ObjectHelper.MapList<TechnicalService, TechnicalServiceViewModel>(filtered);
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

        #region GetTechnicalServiceById
        [HttpPost("GetTechnicalServiceById")]
        public async Task<GetTechnicalServiceByIdParameterResult> GetTechnicalServiceById([FromBody] GetTechnicalServiceByIdParameter body)
        {
            var result = new GetTechnicalServiceByIdParameterResult();

            try
            {
                var entity = await _helper.GetByIdAsync<TechnicalService>(body.Id);

                if (entity != null && entity.ActiveFlg)
                {
                    result.TechnicalService = ObjectHelper.Map<TechnicalService, TechnicalServiceViewModel>(entity);
                    result.Result = true;
                    result.Message = "Success";
                }
                else
                {
                    result.Result = false;
                    result.Message = "TechnicalService not found or inactive.";
                }
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
            }

            return result;
        }


        #endregion

        #region SaveTechnicalService
        [HttpPost("SaveTechnicalService")]
        public async Task<SaveTechnicalServiceParameterResult> SaveTechnicalService([FromBody] SaveTechnicalServiceParameter body)
        {
            var result = new SaveTechnicalServiceParameterResult();

            try
            {
                var entity = ObjectHelper.Map<TechnicalServiceViewModel, TechnicalService>(body.TechnicalService);
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
