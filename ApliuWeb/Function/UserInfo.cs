using ApliuTools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace ApliuWeb
{
    public class UserInfo
    {
        /// <summary>
        /// 登录成功后设置用户信息
        /// </summary>
        public static void SetUserInfo(string UserId, string UserName, string MobileNumber, string Openid, string Unionid)
        {
            Common.SetSessionValue("UserId", UserId);
            Common.SetSessionValue("UserName", UserName);
            Common.SetSessionValue("MobileNumber", MobileNumber);
            Common.SetSessionValue("Openid", Openid);
            Common.SetSessionValue("Unionid", Unionid);
        }

        /// <summary>
        /// 退出 清空用户信息
        /// </summary>
        public static bool Logout()
        {
            try
            {
                SetUserInfo("", "", "", "", "");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool UserCheck(string UserId)
        {
            string regs = string.Format(@"select 1 from ApUserInfo where UserId='{0}';", UserId);
            DataSet ds = DataAccess.Instance.GetData(regs);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
            {
                return true;
            }
            else return false;
        }

        public static bool LoginCheck(string UserId, string Password)
        {
            string regs = string.Format(@"select 1 from ApUserInfo where UserId='{0}' and Password='{1}';", UserId, SecurityHelper.MD5Encrypt(Password, Encoding.UTF8));
            DataSet ds = DataAccess.Instance.GetData(regs);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
            {
                return true;
            }
            else return false;
        }
    }
}