using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public partial class tb_AdminPermission
    {
        public int ID { get; set; }
        public int RoleID { get; set; }
        public int NodeID { get; set; }
        public bool IsDel { get; set; }
    }
}
