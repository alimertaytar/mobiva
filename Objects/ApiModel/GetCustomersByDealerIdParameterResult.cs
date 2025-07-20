using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.ApiModel
{
    public class GetCustomersByDealerIdParameterResult : BaseParameterResult
    {
        public List<CustomerViewModel> Customers { get; set; }
    }
}
