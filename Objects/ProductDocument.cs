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
    
    public partial class ProductDocument
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ProductDocumentTypeId { get; set; }
        public string UId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string Detail { get; set; }
        public int CreateUserId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public bool ActiveFlg { get; set; }
    
        public virtual ProductDocumentType ProductDocumentType { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}
