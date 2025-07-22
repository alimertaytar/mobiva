using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.ViewModel
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public int ProductTypeId { get; set; }
        public int ProductTypeSubId { get; set; }
        public int ProductStatusId { get; set; }
        public string ProductIMEI { get; set; }
        public string ProductBarcode { get; set; }
        public string ProductBarcodeMobiva { get; set; }
        public string Name { get; set; }
        public int DealerId { get; set; }
        public decimal PurchasePriceTL { get; set; }
        public decimal PurchasePriceUSD { get; set; }
        public decimal SellingPriceTL { get; set; }
        public decimal MinSellingPriceTL { get; set; }
        public decimal CurrentUSDPrice { get; set; }
        public int StockNumber { get; set; }
        public int CriticalStock { get; set; }
        public string ProductNote { get; set; }
        public int CreateUserId { get; set; }

        public ProductDetail ProductDetail { get; set; }
    }
}
