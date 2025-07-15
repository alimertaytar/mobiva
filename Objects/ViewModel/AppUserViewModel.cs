using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.ViewModel
{
    public class AppUserViewModel
    {
        public int Id { get; set; }
        public string NameSurname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int AppUserTypeId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public bool ActiveFlg { get; set; }
    }
}
