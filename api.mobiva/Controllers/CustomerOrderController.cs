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
    public class CustomerOrderController : ControllerBase
    {
        private readonly ContextApi _context;
        private readonly ContextApiHelper _helper;

        public CustomerOrderController(ContextApi context)
        {
            _context = context;
            _helper = new ContextApiHelper(_context);
        }

        #region GetCustomerOrders
        [HttpPost("GetCustomerOrders")]
        public async Task<GetCustomerOrdersParameterResult> GetCustomerOrders([FromBody] GetCustomerOrdersParameter param)
        {
            var result = new GetCustomerOrdersParameterResult();

            try
            {
                var filtered = await _helper.GetAllAsync<CustomerOrder>(
                    x => x.ActiveFlg && x.DealerId == param.DealerId
                );

                result.CustomerOrders = ObjectHelper.MapList<CustomerOrder, CustomerOrderViewModel>(filtered);
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

        #region SaveCustomerOrder
        [HttpPost("SaveCustomerOrder")]
        public async Task<SaveCustomerOrderParameterResult> SaveCustomerOrder([FromBody] SaveCustomerOrderParameter param)
        {
            var result = new SaveCustomerOrderParameterResult();
            var entity = ObjectHelper.Map<CustomerOrderViewModel, CustomerOrder>(param.CustomerOrder);
            var opResult = await _helper.SaveAsync(entity);
            result.Result = opResult.Success;
            result.Message = opResult.Message;
            result.Id = opResult.Data;
            return result;
        }


        #endregion

        #region GetCustomerOrderById
        [HttpPost("GetCustomerOrderById")]
        public async Task<GetCustomerOrderByIdParameterResult> GetCustomerOrderById([FromBody] GetCustomerOrderByIdParameter param)
        {
            var result = new GetCustomerOrderByIdParameterResult();

            try
            {
                var entity = await _helper.GetByIdAsync<CustomerOrder>(param.CustomerOrderId);

                if (entity != null && entity.ActiveFlg)
                    result.CustomerOrder = ObjectHelper.Map<CustomerOrder, CustomerOrderViewModel>(entity);

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
