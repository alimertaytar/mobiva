using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.ApiModel
{
    public class GetCustomerByIdParameterResult : BaseParameterResult
    {
        public CustomerViewModel Customer { get; set; }
    }
}
