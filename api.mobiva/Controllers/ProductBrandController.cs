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
    public class ProductBrandController : ControllerBase
    {
        private readonly ContextApi _context;
        private readonly ContextApiHelper _helper;

        public ProductBrandController(ContextApi context)
        {
            _context = context;
            _helper = new ContextApiHelper(_context);
        }

        #region GetProductBrands
        [HttpPost("GetProductBrands")]
        public async Task<GetProductBrandsParameterResult> GetProductBrands()
        {
            var result = new GetProductBrandsParameterResult();

            try
            {
                var filtered = await _helper.GetAllAsync<ProductBrand>();
                result.ProductBrands = ObjectHelper.MapList<ProductBrand, ProductBrandViewModel>(filtered);
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

        #region SaveProductBrand
        [HttpPost("SaveProductBrand")]
        public async Task<SaveProductBrandParameterResult> SaveProductBrand([FromBody] SaveProductBrandParameter param)
        {
            var result = new SaveProductBrandParameterResult();
            var entity = ObjectHelper.Map<ProductBrandViewModel, ProductBrand>(param.ProductBrand);
            var opResult = await _helper.SaveAsync(entity);
            result.Result = opResult.Success;
            result.Message = opResult.Message;
            result.Id = opResult.Data;
            return result;
        }


        #endregion

        #region GetProductBrandById
        [HttpPost("GetProductBrandById")]
        public async Task<GetProductBrandByIdParameterResult> GetProductBrandById([FromBody] GetProductBrandByIdParameter param)
        {
            var result = new GetProductBrandByIdParameterResult();

            try
            {
                var entity = await _helper.GetByIdAsync<ProductBrand>(param.Id);

                if (entity != null)
                    result.ProductBrand = ObjectHelper.Map<ProductBrand, ProductBrandViewModel>(entity);

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
