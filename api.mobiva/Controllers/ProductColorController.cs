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
    public class ProductColorController : ControllerBase
    {
        private readonly ContextApi _context;
        private readonly ContextApiHelper _helper;

        public ProductColorController(ContextApi context)
        {
            _context = context;
            _helper = new ContextApiHelper(_context);
        }

        #region GetProductColors
        [HttpPost("GetProductColors")]
        public async Task<GetProductColorsParameterResult> GetProductColors()
        {
            var result = new GetProductColorsParameterResult();

            try
            {
                var filtered = await _helper.GetAllAsync<ProductColor>(pc => pc.ActiveFlg);
                result.ProductColors = ObjectHelper.MapList<ProductColor, ProductColorViewModel>(filtered);
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

        #region SaveProductColor
        [HttpPost("SaveProductColor")]
        public async Task<SaveProductColorParameterResult> SaveProductColor([FromBody] SaveProductColorParameter param)
        {
            var result = new SaveProductColorParameterResult();
            var entity = ObjectHelper.Map<ProductColorViewModel, ProductColor>(param.ProductColor);
            var opResult = await _helper.SaveAsync(entity);
            result.Result = opResult.Success;
            result.Message = opResult.Message;
            result.Id = opResult.Data;
            return result;
        }


        #endregion

        #region GetProductColorById
        [HttpPost("GetProductColorById")]
        public async Task<GetProductColorByIdParameterResult> GetProductColorById([FromBody] GetProductColorByIdParameter param)
        {
            var result = new GetProductColorByIdParameterResult();

            try
            {
                var entity = await _helper.GetByIdAsync<ProductColor>(param.Id);

                if (entity != null && entity.ActiveFlg)
                    result.ProductColor = ObjectHelper.Map<ProductColor, ProductColorViewModel>(entity);

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
