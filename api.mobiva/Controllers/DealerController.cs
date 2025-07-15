using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Objects;
using Objects.ApiModel;
using Objects.ViewModel;
using System.Linq;
using System.Threading.Tasks;

namespace api.mobiva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealerController : ControllerBase
    {
        private readonly ContextApi _context;
        private readonly ContextApiHelper _helper;

        public DealerController(ContextApi context)
        {
            _context = context;
            _helper = new ContextApiHelper(_context);
        }

        #region GetDealerById
        [HttpPost("GetDealerById")]
        public async Task<GetDealerByIdParameterResult> GetDealerById([FromBody] GetDealerByIdParameter param)
        {
            var result = new GetDealerByIdParameterResult();

            try
            {
                var entity = await _helper.GetByIdAsync<Dealer>(param.Id);

                if (entity != null && entity.ActiveFlg)
                    result.Dealer = ObjectHelper.Map<Dealer, DealerViewModel>(entity);

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

        #region GetDealersByAppUserId
        [HttpPost("GetDealersByAppUserId")]
        public async Task<GetDealersByAppUserIdParameterResult> GetProductModels([FromBody] GetDealersByAppUserIdParameter param)
        {
            var result = new GetDealersByAppUserIdParameterResult();

            try
            {

                var dealerIds = _context.DealerAppUser
                   .Where(x => x.AppUserId == param.AppUserId)
                   .Select(x => x.DealerId);

                var dealers = await _context.Dealer
                    .Where(x => dealerIds.Contains(x.Id))
                    .ToListAsync();

                result.Dealers = ObjectHelper.MapList<Dealer, DealerViewModel>(dealers);

                
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
