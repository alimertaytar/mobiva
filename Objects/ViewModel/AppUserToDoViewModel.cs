using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.ViewModel
{
    public class AppUserToDoViewModel
    {
        public int Id { get; set; }
        public string ToDoDetail { get; set; }
        public int CreateUserId { get; set; }
        public int ToDoUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsDone { get; set; }
        public DateTime DoneDate { get; set; }
        public string DoneDetail { get; set; }
        public bool ActiveFlg { get; set; }
    }

}
