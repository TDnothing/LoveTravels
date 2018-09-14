using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FytMsys.Vector.WeixinLogin
{
    public class WeiXinAccessTokenResult
    {
        public WeiXinAccessTokenModel SuccessResult { get; set; }
        public bool Result { get; set; }

        public WeiXinErrorMsg ErrorResult { get; set; }
    }
}
