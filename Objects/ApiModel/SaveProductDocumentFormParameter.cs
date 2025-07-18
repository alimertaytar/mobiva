﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.ApiModel
{
    public class SaveProductDocumentFormParameter
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ProductDocumentTypeId { get; set; }
        public string Detail { get; set; }
        public int CreateUserId { get; set; }
        public IFormFile File { get; set; }
    }
}
