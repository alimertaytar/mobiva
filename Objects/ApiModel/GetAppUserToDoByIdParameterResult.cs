using Objects.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.ApiModel
{
    public class GetAppUserToDoByIdParameterResult : BaseParameterResult
    {
        public AppUserToDoViewModel AppUserToDo { get; set; }
    }
}
