using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Objects.ViewModel;
using Objects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Objects.ApiModel;

namespace api.mobiva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductStatusController : ControllerBase
    {
        private readonly ContextApi _context;
        private readonly ContextApiHelper _helper;

        public ProductStatusController(ContextApi context)
        {
            _context = context;
            _helper = new ContextApiHelper(_context);
        }

        #region GetProductStatuses
        [HttpPost("GetProductStatuses")]
        public async Task<GetProductStatusesParameterResult> GetProductStatuses()
        {
            var result = new GetProductStatusesParameterResult();

            try
            {
                var filtered = await _helper.GetAllAsync<ProductStatus>(x => x.ActiveFlg);
                result.ProductStatuses = ObjectHelper.MapList<ProductStatus, ProductStatusViewModel>(filtered);

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

        #region SaveProductStatus
        [HttpPost("SaveProductStatus")]
        public async Task<SaveProductStatusParameterResult> SaveProductStatus([FromBody] SaveProductStatusParameter param)
        {
            var result = new SaveProductStatusParameterResult();
            var entity = ObjectHelper.Map<ProductStatusViewModel, ProductStatus>(param.ProductStatus);
            var opResult = await _helper.SaveAsync(entity);
            result.Result = opResult.Success;
            result.Message = opResult.Message;
            result.Id = opResult.Data;
            return result;
        }


        #endregion

        #region GetProductStatusById
        [HttpPost("GetProductStatusById")]
        public async Task<GetProductStatusByIdParameterResult> GetProductStatusById([FromBody] GetProductStatusByIdParameter param)
        {
            var result = new GetProductStatusByIdParameterResult();

            try
            {
                var entity = await _helper.GetByIdAsync<ProductStatus>(param.Id);

                if (entity != null && entity.ActiveFlg)
                    result.ProductStatus = ObjectHelper.Map<ProductStatus, ProductStatusViewModel>(entity);

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
