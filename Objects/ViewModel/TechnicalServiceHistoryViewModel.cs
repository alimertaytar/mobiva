using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.ViewModel
{
    public class TechnicalServiceHistoryViewModel
    {
        public int Id { get; set; }
        public int TechnicalServiceId { get; set; }
        public int TechnicalServiceStatusId { get; set; }
        public string Detail { get; set; }
        public DateTime CreateDate { get; set; }
        public bool ActiveFlg { get; set; }
    }

}
