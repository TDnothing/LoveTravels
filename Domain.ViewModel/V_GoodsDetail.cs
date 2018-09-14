using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public partial class V_GoodsDetail
    {
        public int ID { get; set; }
        public string GoodsNum { get; set; }
        public string GoodsName { get; set; }
        public string SubTitle { get; set; }
        public string GoodsImage { get; set; }
        public int ClassId { get; set; }
        public int ShopId { get; set; }
        public string Specification { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public int Stock { get; set; }
        public int SoldNum { get; set; }
        public int LimitNum { get; set; }
        public string Content { get; set; }
        public string Notice { get; set; }
        public bool IsTop { get; set; }
        public DateTime AddDate { get; set; }
        public bool IsList { get; set; }
        public bool IsSlide { get; set; }
        public DateTime EditDate { get; set; }
        public string ShopName { get; set; }
    }
}
