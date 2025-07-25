using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.ViewModel
{
    public class ProductDetailViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ProductBrandId { get; set; }
        public int ProductModelId { get; set; }
        public int ProductColorId { get; set; }
        public int CustomerIdBuying { get; set; }
        public int CustomerIdSelling { get; set; }
        public string InternalMemoryGB { get; set; }
        public string RamGB { get; set; }
        public string CosmicConditionPercentage { get; set; }
        public string BatteryMah { get; set; }
        public string BatteryPercentage { get; set; }
        public Nullable<System.DateTime> WarrantyEndDate { get; set; }
    }

}
