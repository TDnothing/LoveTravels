using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    /// <summary>
    /// 会员配置信息
    /// </summary>
    [Serializable]
    public class MemberConfig
    {
        public MemberConfig()
        { }
        private int _regstatus = 0;
        private string _regkeywords = "";
        private int _regverify = 0;
        private int _regctrl = 0;
        private int _regemailexpired = 0;
        private int _regemailditto = 0;
        private int _emaillogin = 0;
        private int _regrules = 0;
        private string _regrulestxt = "";
        private int _regmsgstatus = 0;
        private string _regmsgtxt = "";
        private int _invitecodeexpired = 0;
        private int _invitecodecount = 0;
        private int _invitecodenum = 10;
        private decimal _pointcashrate = 0;
        private int _pointinvitenum = 0;

        /// <summary>
        /// 新用户注册设置0不允许注册,1允许注册,2仅通过邀请注册
        /// </summary>
        public int Regstatus
        {
            get { return _regstatus; }
            set { _regstatus = value; }
        }
        /// <summary>
        /// 用户名保留关健字
        /// </summary>
        public string Regkeywords
        {
            get { return _regkeywords; }
            set { _regkeywords = value; }
        }
        /// <summary>
        /// 新用户注册验证0无,1Email验证,2人工审核
        /// </summary>
        public int Regverify
        {
            get { return _regverify; }
            set { _regverify = value; }
        }
        /// <summary>
        /// IP注册间隔限制0不限制(小时)
        /// </summary>
        public int Regctrl
        {
            get { return _regctrl; }
            set { _regctrl = value; }
        }
        /// <summary>
        /// Email验证请求有效期0不限制(天)
        /// </summary>
        public int Regemailexpired
        {
            get { return _regemailexpired; }
            set { _regemailexpired = value; }
        }
        /// <summary>
        /// 允许同一Email注册不同用户0不允许1允许
        /// </summary>
        public int Regemailditto
        {
            get { return _regemailditto; }
            set { _regemailditto = value; }
        }
        /// <summary>
        /// 允许Email登录0不允许1允许
        /// </summary>
        public int Emaillogin
        {
            get { return _emaillogin; }
            set { _emaillogin = value; }
        }
        /// <summary>
        /// 注册许可协议0否1是
        /// </summary>
        public int Regrules
        {
            get { return _regrules; }
            set { _regrules = value; }
        }
        /// <summary>
        /// 许可协议内容
        /// </summary>
        public string Regrulestxt
        {
            get { return _regrulestxt; }
            set { _regrulestxt = value; }
        }
        /// <summary>
        /// 注册欢迎短信息0否1是
        /// </summary>
        public int Regmsgstatus
        {
            get { return _regmsgstatus; }
            set { _regmsgstatus = value; }
        }
        /// <summary>
        /// 欢迎短信息内容
        /// </summary>
        public string Regmsgtxt
        {
            get { return _regmsgtxt; }
            set { _regmsgtxt = value; }
        }
        /// <summary>
        /// 邀请码使用期限(天)0不限制
        /// </summary>
        public int Invitecodeexpired
        {
            get { return _invitecodeexpired; }
            set { _invitecodeexpired = value; }
        }
        /// <summary>
        /// 邀请码可使用次数0无限制
        /// </summary>
        public int Invitecodecount
        {
            get { return _invitecodecount; }
            set { _invitecodecount = value; }
        }
        /// <summary>
        /// 每天可申请的邀请码数量0不限制
        /// </summary>
        public int Invitecodenum
        {
            get { return _invitecodenum; }
            set { _invitecodenum = value; }
        }
        /// <summary>
        /// 现金/积分兑换比例0禁用
        /// </summary>
        public decimal Pointcashrate
        {
            get { return _pointcashrate; }
            set { _pointcashrate = value; }
        }
        /// <summary>
        /// 邀请注册获得积分
        /// </summary>
        public int Pointinvitenum
        {
            get { return _pointinvitenum; }
            set { _pointinvitenum = value; }
        }
    }
}
