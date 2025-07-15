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
    public class ProductDocumentTypeController : ControllerBase
    {
        private readonly ContextApi _context;
        private readonly ContextApiHelper _helper;

        public ProductDocumentTypeController(ContextApi context)
        {
            _context = context;
            _helper = new ContextApiHelper(_context);
        }

        #region GetProductDocumentTypes
        [HttpPost("GetProductDocumentTypes")]
        public async Task<GetProductDocumentTypesParameterResult> GetProductDocumentTypes()
        {
            var result = new GetProductDocumentTypesParameterResult();

            try
            {
                var filtered = await _helper.GetAllAsync<ProductDocumentType>(x => x.ActiveFlg);
                result.ProductDocumentTypes = ObjectHelper.MapList<ProductDocumentType, ProductDocumentTypeViewModel>(filtered);
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

        #region SaveProductDocumentType
        [HttpPost("SaveProductDocumentType")]
        public async Task<SaveProductDocumentTypeParameterResult> SaveProductDocumentType([FromBody] SaveProductDocumentTypeParameter param)
        {
            var result = new SaveProductDocumentTypeParameterResult();
            var entity = ObjectHelper.Map<ProductDocumentTypeViewModel, ProductDocumentType>(param.ProductDocumentType);
            var opResult = await _helper.SaveAsync(entity);
            result.Result = opResult.Success;
            result.Message = opResult.Message;
            result.Id = opResult.Data;
            return result;
        }


        #endregion

        #region GetProductDocumentTypeById
        [HttpPost("GetProductDocumentTypeById")]
        public async Task<GetProductDocumentTypeByIdParameterResult> GetProductDocumentTypeById([FromBody] GetProductDocumentTypeByIdParameter param)
        {
            var result = new GetProductDocumentTypeByIdParameterResult();

            try
            {
                var entity = await _helper.GetByIdAsync<ProductDocumentType>(param.Id);

                if (entity != null && entity.ActiveFlg)
                    result.ProductDocumentType = ObjectHelper.Map<ProductDocumentType, ProductDocumentTypeViewModel>(entity);

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
