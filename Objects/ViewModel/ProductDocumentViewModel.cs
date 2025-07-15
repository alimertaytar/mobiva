using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.ViewModel
{
    public class ProductDocumentViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ProductDocumentTypeId { get; set; }
        public string UId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string Detail { get; set; }
        public int CreateUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public bool ActiveFlg { get; set; }
    }
}
