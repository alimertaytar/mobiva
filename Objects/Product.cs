//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Objects
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.ProductDetail = new HashSet<ProductDetail>();
            this.ProductFeature = new HashSet<ProductFeature>();
            this.Sale = new HashSet<Sale>();
        }
    
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
        public System.DateTime CreateDate { get; set; }
        public bool IsOnlineSales { get; set; }
        public bool ActiveFlg { get; set; }
    
        public virtual Dealer Dealer { get; set; }
        public virtual ProductType ProductType { get; set; }
        public virtual ProductTypeSub ProductTypeSub { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductDetail> ProductDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductFeature> ProductFeature { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sale { get; set; }
        public virtual ProductStatus ProductStatus { get; set; }
    }
}
