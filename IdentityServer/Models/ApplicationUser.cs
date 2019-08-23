using DevExpress.Xpo;
using IdentityServer4.Xpo.Entities;

namespace IdentityServer.Models
{
    // Add profile data for application users by adding properties to the BimUser class
    /// <summary>
    /// 用户
    /// </summary>
    [Persistent("User")]
    public class ApplicationUser : XPGuidObject
    {

        public ApplicationUser() : base(Session.DefaultSession)
        {
        }

        public ApplicationUser(Session session) : base(session)
        {
        }

        #region FILED
        /// <summary>
        /// 登录名
        /// </summary>
        [Indexed(Unique =true), Size(20)]
        public string LoginName { get; set; }
        /// <summary>
        /// 
        /// 姓名
        /// </summary>
        [Size(20)]
        public string Name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        //[ValueConverter(typeof(Encryption))]
        [Persistent("Password"), Size(50)]
        private string _Password;
        [NonPersistent]
        [DisplayName("密码")]
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }// ConvertHelper.BimMD5(value); }
        }
        /// <summary>
        /// 民族
        /// </summary>
        [DisplayName("民族")]
        public string Nation { get; set; }
        /// <summary>
        /// 籍贯
        /// </summary>
        [DisplayName("籍贯")]
        public string NativePlace { get; set; }
        /// <summary>
        /// true 男 false 女
        /// </summary>
        [DisplayName("性别")]
        public bool? Sex { get; set; }
        /// <summary>
        /// 婚否 true已婚 false 未婚
        /// </summary>
        [DisplayName("婚否")]
        public bool? Marriage { get; set; }

        #region 联系信息
        /// <summary>
        /// 手机号
        /// </summary>
        [Size(11)]
        public string Mobile { get; set; }

        /// <summary>
        /// 固定电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        [Size(30)]
        public string Email { get; set; }
        /// <summary>
        /// 微信号
        /// </summary>
        public string WeChat { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }


        /// <summary>
        /// 联系信息可见性
        /// 以10010表示，1为公开，0为隐藏
        /// 手机、固话、邮箱、微信、QQ
        /// </summary>
        public string Visible { get; set; }
        #endregion

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 默认语言
        /// </summary>
        [Size(30)]
        public string Culture { get; set; }

        /// <summary>
        /// 是否无效
        /// </summary>
        [DisplayName("是否无效")]
        public bool Invalid { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Size(200)]
        public string Description { get; set; }
        #endregion

    }
}
