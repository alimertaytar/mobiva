using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.ViewModel
{
    public class ProductModelViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductBrandId { get; set; }
        public string ProductBrand { get; set; }
        public bool ActiveFlg { get; set; }
    }

}
