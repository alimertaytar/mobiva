using Objects.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.ApiModel
{
    public class GetProductsParameterResult : BaseParameterResult
    {
        public List<ProductViewModel> Products { get; set; }
    }
}
