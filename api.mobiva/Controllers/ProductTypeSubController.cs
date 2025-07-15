using Microsoft.AspNetCore.Mvc;
using Objects.ViewModel;
using Objects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace api.mobiva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypeSubController : ControllerBase
    {
        private readonly ContextApi _context;
        private readonly ContextApiHelper _helper;

        public ProductTypeSubController(ContextApi context)
        {
            _context = context;
            _helper = new ContextApiHelper(_context);
        }

        #region BaseParameterResult
        public class BaseParameterResult
        {
            public bool Result { get; set; }
            public string Message { get; set; }
        }
        #endregion

        #region GetProductTypeSubs
        [HttpPost("GetProductTypeSubs")]
        public async Task<GetProductTypeSubsParameterResult> GetProductTypeSubs()
        {
            var result = new GetProductTypeSubsParameterResult();

            try
            {
                var filtered = await _helper.GetAllAsync<ProductTypeSub>(x => x.ActiveFlg);
                result.ProductTypeSubs = ObjectHelper.MapList<ProductTypeSub, ProductTypeSubViewModel>(filtered);
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

        public class GetProductTypeSubsParameterResult : BaseParameterResult
        {
            public List<ProductTypeSubViewModel> ProductTypeSubs { get; set; }
        }
        #endregion

        #region SaveProductTypeSub
        [HttpPost("SaveProductTypeSub")]
        public async Task<SaveProductTypeSubParameterResult> SaveProductTypeSub([FromBody] SaveProductTypeSubParameter param)
        {
            var result = new SaveProductTypeSubParameterResult();
            var entity = ObjectHelper.Map<ProductTypeSubViewModel, ProductTypeSub>(param.ProductTypeSub);
            var opResult = await _helper.SaveAsync(entity);
            result.Result = opResult.Success;
            result.Message = opResult.Message;
            result.Id = opResult.Data;
            return result;
        }

        public class SaveProductTypeSubParameter
        {
            public ProductTypeSubViewModel ProductTypeSub { get; set; }
        }

        public class SaveProductTypeSubParameterResult : BaseParameterResult
        {
            public int Id { get; set; }
        }
        #endregion


        #region GetProductTypeSubById
        [HttpPost("GetProductTypeSubById")]
        public async Task<GetProductTypeSubByIdParameterResult> GetProductTypeSubById([FromBody] GetProductTypeSubByIdParameter param)
        {
            var result = new GetProductTypeSubByIdParameterResult();

            try
            {
                var entity = await _helper.GetByIdAsync<ProductTypeSub>(param.Id);

                if (entity != null && entity.ActiveFlg)
                    result.ProductTypeSub = ObjectHelper.Map<ProductTypeSub, ProductTypeSubViewModel>(entity);

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

        public class GetProductTypeSubByIdParameter
        {
            public int Id { get; set; }
        }

        public class GetProductTypeSubByIdParameterResult : BaseParameterResult
        {
            public ProductTypeSubViewModel ProductTypeSub { get; set; }
        }
        #endregion
    }
}
