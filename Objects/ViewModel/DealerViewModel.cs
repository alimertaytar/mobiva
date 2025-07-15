using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.ViewModel
{
    public class DealerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int ProvinceId { get; set; }
        public int CountyId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public bool ActiveFlg { get; set; }
    }
}
