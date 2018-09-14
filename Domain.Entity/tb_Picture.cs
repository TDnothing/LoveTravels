using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public partial class tb_Picture
    {
        public int ID { get; set; }
        public int ClassId { get; set; }
        public string Title { get; set; }
        public string BigImg { get; set; }
        public string SmallImg { get; set; }
        public int Flag { get; set; }
        public bool IsCover { get; set; }
        public int Sort { get; set; }
    }
}
