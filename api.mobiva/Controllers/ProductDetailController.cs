using Microsoft.AspNetCore.Mvc;
using Objects.ViewModel;
using Objects;
using System.Threading.Tasks;
using Objects.ApiModel;

namespace api.mobiva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDetailController : ControllerBase
    {
        private readonly ContextApi _context;
        private readonly ContextApiHelper _helper;

        public ProductDetailController(ContextApi context)
        {
            _context = context;
            _helper = new ContextApiHelper(_context);
        }

        #region GetProductDetailByProductId
        [HttpPost("GetProductDetailByProductId")]
        public async Task<GetProductDetailByProductIdParameterResult> GetProductDetailByProductId([FromBody] GetProductDetailByProductIdParameter param)
        {
            var result = new GetProductDetailByProductIdParameterResult();

            try
            {
                var entity = await _helper.GetSingleAsync<ProductDetail>(
                    x => x.ProductId == param.ProductId
                );

                if (entity != null)
                    result.ProductDetail = ObjectHelper.Map<ProductDetail, ProductDetailViewModel>(entity);

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

        #region SaveProductDetail
        [HttpPost("SaveProductDetail")]
        public async Task<SaveProductDetailParameterResult> SaveProductDetail([FromBody] SaveProductDetailParameter param)
        {
            var result = new SaveProductDetailParameterResult();
            var entity = ObjectHelper.Map<ProductDetailViewModel, ProductDetail>(param.ProductDetail);
            var opResult = await _helper.SaveAsync(entity);
            result.Result = opResult.Success;
            result.Message = opResult.Message;
            result.Id = opResult.Data;
            return result;
        }


        #endregion
    }
}
