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
    public class ProductFeatureListController : ControllerBase
    {
        private readonly ContextApi _context;
        private readonly ContextApiHelper _helper;

        public ProductFeatureListController(ContextApi context)
        {
            _context = context;
            _helper = new ContextApiHelper(_context);
        }

        #region GetProductFeatureLists
        [HttpPost("GetProductFeatureLists")]
        public async Task<GetProductFeatureListsParameterResult> GetProductFeatureLists()
        {
            var result = new GetProductFeatureListsParameterResult();

            try
            {
                var filtered = await _helper.GetAllAsync<ProductFeatureList>(x => x.ActiveFlg);
                result.ProductFeatureLists = ObjectHelper.MapList<ProductFeatureList, ProductFeatureListViewModel>(filtered);
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

        #region SaveProductFeatureList
        [HttpPost("SaveProductFeatureList")]
        public async Task<SaveProductFeatureListParameterResult> SaveProductFeatureList([FromBody] SaveProductFeatureListParameter param)
        {
            var result = new SaveProductFeatureListParameterResult();
            var entity = ObjectHelper.Map<ProductFeatureListViewModel, ProductFeatureList>(param.ProductFeatureList);
            var opResult = await _helper.SaveAsync(entity);
            result.Result = opResult.Success;
            result.Message = opResult.Message;
            result.Id = opResult.Data;
            return result;
        }


        #endregion

        #region GetProductFeatureListById
        [HttpPost("GetProductFeatureListById")]
        public async Task<GetProductFeatureListByIdParameterResult> GetProductFeatureListById([FromBody] GetProductFeatureListByIdParameter param)
        {
            var result = new GetProductFeatureListByIdParameterResult();

            try
            {
                var entity = await _helper.GetByIdAsync<ProductFeatureList>(param.Id);

                if (entity != null && entity.ActiveFlg)
                    result.ProductFeatureList = ObjectHelper.Map<ProductFeatureList, ProductFeatureListViewModel>(entity);

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
