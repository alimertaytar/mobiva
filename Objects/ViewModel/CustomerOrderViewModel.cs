using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.ViewModel
{
    public class CustomerOrderViewModel
    {
        public int Id { get; set; }
        public int DealerId { get; set; }
        public int CustomerId { get; set; }
        public string Customer { get; set; }
        public string OrderNote { get; set; }
        public int CreateUserId { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public bool ActiveFlg { get; set; }
    }

}
