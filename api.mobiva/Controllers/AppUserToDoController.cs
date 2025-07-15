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
    public class AppUserToDoController : ControllerBase
    {
        private readonly ContextApi _context;
        private readonly ContextApiHelper _helper;

        public AppUserToDoController(ContextApi context)
        {
            _context = context;
            _helper = new ContextApiHelper(_context);
        }

        #region GetAppUserToDos
        [HttpPost("GetAppUserToDos")]
        public async Task<GetAppUserToDosParameterResult> GetAppUserToDos([FromBody] GetAppUserToDosParameter param)
        {
            var result = new GetAppUserToDosParameterResult();

            try
            {
                var filtered = await _helper.GetAllAsync<AppUserToDo>(
                    x => x.ActiveFlg && x.ToDoUserId == param.AppUserId
                );

                result.AppUserToDos = ObjectHelper.MapList<AppUserToDo, AppUserToDoViewModel>(filtered);
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

        #region SaveAppUserToDo
        [HttpPost("SaveAppUserToDo")]
        public async Task<SaveAppUserToDoParameterResult> SaveAppUserToDo([FromBody] SaveAppUserToDoParameter param)
        {
            var result = new SaveAppUserToDoParameterResult();
            var entity = ObjectHelper.Map<AppUserToDoViewModel, AppUserToDo>(param.AppUserToDo);
            var opResult = await _helper.SaveAsync(entity);
            result.Result = opResult.Success;
            result.Message = opResult.Message;
            result.Id = opResult.Data;
            return result;
        }
        #endregion

        #region GetAppUserToDoById
        [HttpPost("GetAppUserToDoById")]
        public async Task<GetAppUserToDoByIdParameterResult> GetAppUserToDoById([FromBody] GetAppUserToDoByIdParameter param)
        {
            var result = new GetAppUserToDoByIdParameterResult();

            try
            {
                var entity = await _helper.GetByIdAsync<AppUserToDo>(param.Id);

                if (entity != null && entity.ActiveFlg)
                    result.AppUserToDo = ObjectHelper.Map<AppUserToDo, AppUserToDoViewModel>(entity);

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
