using Objects.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.ApiModel
{
    public class GetProductTypeSubsParameterResult : BaseParameterResult
    {
        public List<ProductTypeSubViewModel> ProductTypeSubs { get; set; }
    }
}
