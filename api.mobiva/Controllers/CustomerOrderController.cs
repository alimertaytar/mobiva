using Microsoft.AspNetCore.Mvc;
using Objects.ViewModel;
using Objects;
using System.Collections.Generic;
using System.Threading.Tasks;
using Objects.ApiModel;
using System;
using System.Linq;

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

        [HttpPost("GetCustomerOrdersByDealerId")]
        public async Task<GetCustomerOrdersByDealerIdParameterResult> GetCustomerOrdersByDealerId([FromBody] GetCustomerOrdersByDealerIdParameter param)
        {
            var result = new GetCustomerOrdersByDealerIdParameterResult();

            try
            {
                var orders = await _helper.GetAllAsync<CustomerOrder>(x => x.DealerId == param.DealerId);
                var customers = await _helper.GetAllAsync<Customer>();
                var appUsers = await _helper.GetAllAsync<AppUser>();

                var mappedOrders = orders.Select(order =>
                {
                    var customerName = customers.FirstOrDefault(c => c.Id == order.CustomerId)?.Name ?? "Bilinmiyor";
                    var createUserName = appUsers.FirstOrDefault(u => u.Id == order.CreateUserId)?.NameSurname ?? "Bilinmiyor";

                    return new CustomerOrderViewModel
                    {
                        Id = order.Id,
                        DealerId = order.DealerId,
                        CustomerId = order.CustomerId,
                        Customer = customerName,
                        OrderNote = order.OrderNote,
                        CreateUserId = order.CreateUserId,
                        CreateUser = createUserName,
                        CreateDate = order.CreateDate,
                        ActiveFlg = order.ActiveFlg
                    };
                }).ToList();

                result.CustomerOrders = mappedOrders;
                result.Result = true;
                result.Message = "Success";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Veriler alınırken hata oluştu.";
                result.InternalMessage = ex.ToString();
            }

            return result;
        }


        #region SaveCustomerOrder
        [HttpPost("SaveCustomerOrder")]
        public async Task<SaveCustomerOrderParameterResult> SaveCustomerOrder([FromBody] SaveCustomerOrderParameter param)
        {
            var result = new SaveCustomerOrderParameterResult();

            try
            {
                var entity = ObjectHelper.Map<CustomerOrderViewModel, CustomerOrder>(param.CustomerOrder);

                var opResult = await _helper.SaveAsync(entity);

                result.Result = opResult.Success;
                result.Message = opResult.Message;
                result.Id = opResult.Data;
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
            }

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
                var entity = await _helper.GetByIdAsync<CustomerOrder>(param.Id);

                if (entity != null)
                {
                    result.CustomerOrder = ObjectHelper.Map<CustomerOrder, CustomerOrderViewModel>(entity);
                    result.Result = true;
                    result.Message = "Kayıt getirildi.";
                }
                else
                {
                    result.Result = false;
                    result.Message = "Kayıt bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
            }

            return result;
        }
        #endregion

    }
}
