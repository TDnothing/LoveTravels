using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    /// <summary>
    /// 用户临时注册实体
    /// </summary>
    public class UserRegs
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string doctorMobilePhone { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string YzCode { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string doctorRealName { get; set; }
        /// <summary>
        /// 所在医院
        /// </summary>
        public string doctorHospital { get; set; }
        /// <summary>
        /// 工作地点编码  
        /// </summary>
        public string workLocation { get; set; }
        /// <summary>
        /// 工作地点
        /// </summary>
        public string workLocationName { get; set; }
        /// <summary>
        /// 科室
        /// </summary>
        public string doctorOffice { get; set; }
        /// <summary>
        /// 大科室编码   
        /// </summary>
        public string doctorOffice2 { get; set; }
        /// <summary>
        /// 科室编码
        /// </summary>
        public string doctorOfficeCode { get; set; }
        /// <summary>
        /// 大科室编码
        /// </summary>
        public string doctorOffice2Code { get; set; }
        /// <summary>
        /// 职称
        /// </summary>
        public string doctorTitle { get; set; }
        /// <summary>
        /// 职称编码
        /// </summary>
        public string doctorTitleCode { get; set; }
        /// <summary>
        /// 专长
        /// </summary>
        public string doctorSpecially { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string ACADEMIC_JOB { get; set; }
        /// <summary>
        /// 经历
        /// </summary>
        public string RESUME_CONTENT { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string doctorEmail { get; set; }
        /// <summary>
        /// 固定电话
        /// </summary>
        public string doctorTelephone { get; set; }

        /// <summary>
        /// 会诊中心id
        /// </summary>
        public string CONSULTATION_CENTER_ID { get; set; }

        /// <summary>
        /// 医生照片
        /// </summary>
        public string doctorPicture { get; set; }

        /// <summary>
        /// 医生小头像
        /// </summary>
        public string doctorClientBackground { get; set; }

        /// <summary>
        /// 医生大头像
        /// </summary>
        public string doctorBigIconbackground { get; set; }

        /// <summary>
        /// 身份证医生证合拍
        /// </summary>
        public string doctorCertificate { get; set; }

        /// <summary>
        /// 客户id
        /// </summary>
        public string customerId { get; set; }
        /// <summary>
        /// 多美号
        /// </summary>
        public string customerAccounts { get; set; }
        /// <summary>
        /// 终端标记
        /// </summary>
        public string validMark { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        public int EXPERIENCE { get; set; }

        
    }
}
