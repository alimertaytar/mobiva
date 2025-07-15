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
    public class CustomerController : ControllerBase
    {
        private readonly ContextApi _context;
        private readonly ContextApiHelper _helper;

        public CustomerController(ContextApi context)
        {
            _context = context;
            _helper = new ContextApiHelper(_context);
        }

        #region GetCustomers
        [HttpPost("GetCustomers")]
        public async Task<GetCustomersParameterResult> GetCustomers([FromBody] GetCustomersParameter param)
        {
            var result = new GetCustomersParameterResult();

            try
            {
                var filtered = await _helper.GetAllAsync<Customer>(
                    x => x.ActiveFlg && x.DealerId == param.DealerId
                );

                result.Customers = ObjectHelper.MapList<Customer, CustomerViewModel>(filtered);
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

        #region SaveCustomer
        [HttpPost("SaveCustomer")]
        public async Task<SaveCustomerParameterResult> SaveCustomer([FromBody] SaveCustomerParameter param)
        {
            var result = new SaveCustomerParameterResult();
            var entity = ObjectHelper.Map<CustomerViewModel, Customer>(param.Customer);
            var opResult = await _helper.SaveAsync(entity);
            result.Result = opResult.Success;
            result.Message = opResult.Message;
            result.Id = opResult.Data;
            return result;
        }

       

        #endregion

        #region GetCustomerById
        [HttpPost("GetCustomerById")]
        public async Task<GetCustomerByIdParameterResult> GetCustomerById([FromBody] GetCustomerByIdParameter param)
        {
            var result = new GetCustomerByIdParameterResult();

            try
            {
                var entity = await _helper.GetByIdAsync<Customer>(param.CustomerId);

                if (entity != null && entity.ActiveFlg)
                    result.Customer = ObjectHelper.Map<Customer, CustomerViewModel>(entity);

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
