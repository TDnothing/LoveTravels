using System.Web.Configuration;
using System.Configuration;
using FytMsys.Common;

namespace upacp_sdk_net.com.unionpay.sdk{

    public class SDKConfig
    {
        private static Configuration config = WebConfigurationManager.OpenWebConfiguration("~");

        private static string signCertPath =
            System.Web.HttpContext.Current.Server.MapPath(ConfigHelper.GetConfigString("sdk.signCert.path")); //config.AppSettings.Settings["sdk.signCert.path"].Value;  //功能：读取配置文件获取签名证书路径
        private static string signCertPwd = ConfigHelper.GetConfigString("sdk.signCert.pwd"); //config.AppSettings.Settings["sdk.signCert.pwd"].Value;//功能：读取配置文件获取签名证书密码
        private static string validateCertDir = 
            System.Web.HttpContext.Current.Server.MapPath(ConfigHelper.GetConfigString("sdk.validateCert.dir"));// config.AppSettings.Settings["sdk.validateCert.dir"].Value;//功能：读取配置文件获取验签目录
        public static string encryptCert =
            System.Web.HttpContext.Current.Server.MapPath(ConfigHelper.GetConfigString("sdk.encryptCert.path")); //config.AppSettings.Settings["sdk.encryptCert.path"].Value;  //功能：加密公钥证书路径
        
        private static string cardRequestUrl = config.AppSettings.Settings["sdk.cardRequestUrl"].Value;  //功能：有卡交易路径;
        private static string appRequestUrl = config.AppSettings.Settings["sdk.appRequestUrl"].Value;  //功能：appj交易路径;
        private static string singleQueryUrl = config.AppSettings.Settings["sdk.singleQueryUrl"].Value; //功能：读取配置文件获取交易查询地址
        private static string fileTransUrl = config.AppSettings.Settings["sdk.fileTransUrl"].Value;  //功能：读取配置文件获取文件传输类交易地址
        private static string frontTransUrl = config.AppSettings.Settings["sdk.frontTransUrl"].Value; //功能：读取配置文件获取前台交易地址
        private static string backTransUrl = config.AppSettings.Settings["sdk.backTransUrl"].Value;//功能：读取配置文件获取后台交易地址
        private static string batTransUrl = config.AppSettings.Settings["sdk.batTransUrl"].Value;//功能：读取配批量交易地址

        private static string frontUrl = ConfigHelper.GetConfigString("frontUrl"); //config.AppSettings.Settings["frontUrl"].Value;//功能：读取配置文件获取前台通知地址
        private static string backUrl = ConfigHelper.GetConfigString("backUrl"); //config.AppSettings.Settings["backUrl"].Value;//功能：读取配置文件获取前台通知地址

        public static string CardRequestUrl
        {
            get { return SDKConfig.cardRequestUrl; }
            set { SDKConfig.cardRequestUrl = value; }
        }
        public static string AppRequestUrl
        {
            get { return SDKConfig.appRequestUrl; }
            set { SDKConfig.appRequestUrl = value; }
        }

        public static string FrontTransUrl
        {
            get { return SDKConfig.frontTransUrl; }
            set { SDKConfig.frontTransUrl = value; }
        }
        public static string EncryptCert
        {
            get { return SDKConfig.encryptCert; }
            set { SDKConfig.encryptCert = value; }
        }


        public static string BackTransUrl
        {
            get { return SDKConfig.backTransUrl; }
            set { SDKConfig.backTransUrl = value; }
        }

        public static string SingleQueryUrl
        {
            get { return SDKConfig.singleQueryUrl; }
            set { SDKConfig.singleQueryUrl = value; }
        }

        public static string FileTransUrl
        {
            get { return SDKConfig.fileTransUrl; }
            set { SDKConfig.fileTransUrl = value; }
        }

        public static string SignCertPath
        {
            get { return SDKConfig.signCertPath; }
            set { SDKConfig.signCertPath = value; }
        }

        public static string SignCertPwd
        {
            get { return SDKConfig.signCertPwd; }
            set { SDKConfig.signCertPwd = value; }
        }

        public static string ValidateCertDir
        {
            get { return SDKConfig.validateCertDir; }
            set { SDKConfig.validateCertDir = value; }
        }
        public static string BatTransUrl
        {
            get { return SDKConfig.batTransUrl; }
            set { SDKConfig.batTransUrl = value; }
        }
        public static string BackUrl
        {
            get { return SDKConfig.backUrl; }
            set { SDKConfig.backUrl = value; }
        }
        public static string FrontUrl
        {
            get { return SDKConfig.frontUrl; }
            set { SDKConfig.frontUrl = value; }
        }
    }
}