using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.ViewModel
{
    public class TechnicalServiceViewModel
    {
        public int Id { get; set; }
        public int DealerId { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int TechnicalServiceStatusId { get; set; }
        public string ProblemDetail { get; set; }
        public string DealerNote { get; set; }
        public string Password { get; set; }
        public string Pattern { get; set; }
        public decimal AstimatedPrice { get; set; }
        public decimal ApprovedPrice { get; set; }
        public decimal CostPrice { get; set; }
        public string TechnicalRepairShop { get; set; }
        public int TechnicalServiceUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public bool ActiveFlg { get; set; }
    }

}
