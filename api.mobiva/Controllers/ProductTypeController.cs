using Microsoft.AspNetCore.Mvc;
using Objects.ViewModel;
using Objects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Objects.ApiModel;

namespace api.mobiva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypeController : ControllerBase
    {
        private readonly ContextApi _context;
        private readonly ContextApiHelper _helper;

        public ProductTypeController(ContextApi context)
        {
            _context = context;
            _helper = new ContextApiHelper(_context);
        }

        #region GetProductTypes
        [HttpPost("GetProductTypes")]
        public async Task<GetProductTypesParameterResult> GetProductTypes()
        {
            var result = new GetProductTypesParameterResult();

            try
            {
                var filtered = await _helper.GetAllAsync<ProductType>(x => x.ActiveFlg);
                result.ProductTypes = ObjectHelper.MapList<ProductType, ProductTypeViewModel>(filtered);
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

        #region SaveProductType
        [HttpPost("SaveProductType")]
        public async Task<SaveProductTypeParameterResult> SaveProductType([FromBody] SaveProductTypeParameter param)
        {
            var result = new SaveProductTypeParameterResult();
            var entity = ObjectHelper.Map<ProductTypeViewModel, ProductType>(param.ProductType);
            var opResult = await _helper.SaveAsync(entity);
            result.Result = opResult.Success;
            result.Message = opResult.Message;
            result.Id = opResult.Data;
            return result;
        }


        #endregion

        #region GetProductTypeById
        [HttpPost("GetProductTypeById")]
        public async Task<GetProductTypeByIdParameterResult> GetProductTypeById([FromBody] GetProductTypeByIdParameter param)
        {
            var result = new GetProductTypeByIdParameterResult();

            try
            {
                var entity = await _helper.GetByIdAsync<ProductType>(param.Id);

                if (entity != null && entity.ActiveFlg)
                    result.ProductType = ObjectHelper.Map<ProductType, ProductTypeViewModel>(entity);

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
