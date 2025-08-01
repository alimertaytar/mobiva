using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.ApiModel
{
    public class GetProductsInventoryByDealerIdParameterResult : BaseParameterResult
    {
        public List<GetProductsSummaryByDealerId_Result> GetProductsSummaryByDealerId { get; set; }
        public List<GetProductsSummaryDetailByDealerId_Result> GetProductsSummaryDetailByDealerId { get; set; }

       

    }

}
