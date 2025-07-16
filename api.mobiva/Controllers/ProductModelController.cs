using Microsoft.AspNetCore.Mvc;
using Objects.ViewModel;
using Objects;
using System.Collections.Generic;
using System.Threading.Tasks;
using Objects.ApiModel;
using System.Linq;

namespace api.mobiva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductModelController : ControllerBase
    {
        private readonly ContextApi _context;
        private readonly ContextApiHelper _helper;

        public ProductModelController(ContextApi context)
        {
            _context = context;
            _helper = new ContextApiHelper(_context);
        }

        #region GetProductModels
        [HttpPost("GetProductModels")]
        public async Task<GetProductModelsParameterResult> GetProductModels([FromBody] GetProductModelsParameter param)
        {
            var result = new GetProductModelsParameterResult();

            try
            {
                var productBrands = await _helper.GetAllAsync<ProductBrand>();
                var filtered = await _helper.GetAllAsync<ProductModel>(
                    x => x.ActiveFlg && x.ProductBrandId == param.ProductBrandId
                );

                result.ProductModels = ObjectHelper.MapList<ProductModel, ProductModelViewModel>(filtered);

                result.ProductModels = filtered.Select(m => new ProductModelViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    ProductBrandId = m.ProductBrandId,
                    ProductBrand = productBrands.FirstOrDefault(b => b.Id == m.ProductBrandId)?.Name,
                    ActiveFlg = m.ActiveFlg
                }).ToList();

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

        #region GetProductModelById
        [HttpPost("GetProductModelById")]
        public async Task<GetProductModelByIdParameterResult> GetProductModelById([FromBody] GetProductModelByIdParameter param)
        {
            var result = new GetProductModelByIdParameterResult();

            try
            {
                var entity = await _helper.GetByIdAsync<ProductModel>(param.Id);

                if (entity != null && entity.ActiveFlg)
                    result.ProductModel = ObjectHelper.Map<ProductModel, ProductModelViewModel>(entity);

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

        #region SaveProductModel
        [HttpPost("SaveProductModel")]
        public async Task<SaveProductModelParameterResult> SaveProductModel([FromBody] SaveProductModelParameter param)
        {
            var result = new SaveProductModelParameterResult();
            var entity = ObjectHelper.Map<ProductModelViewModel, ProductModel>(param.ProductModel);
            var opResult = await _helper.SaveAsync(entity);
            result.Result = opResult.Success;
            result.Message = opResult.Message;
            result.Id = opResult.Data;
            return result;
        }


        #endregion

    }
}
