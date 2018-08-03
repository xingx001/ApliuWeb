using ApliuTools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace ApliuWeb
{
    /// <summary>
    /// 用户登录的Session对象
    /// </summary>
    public class UserSession
    {
        public string UserId;
        public string UserName;
        public string MobileNumber;
        public string Openid;
        public string Unionid;
        public string hubConnectionId;
    }

    public class UserInfo
    {
        /// <summary>
        /// 登录成功后设置用户信息
        /// </summary>
        /// <param name="userSession"></param>
        public static void SetUserInfo(UserSession userSession)
        {
            if (HttpContext.Current.Session["UserSession"] == null)
            {
                HttpContext.Current.Session.Add("UserSession", userSession);
            }
            else
            {
                HttpContext.Current.Session["UserSession"] = userSession;
            }
        }

        /// <summary>
        /// 登录成功后设置用户信息
        /// </summary>
        public static void SetUserInfo(string UserId, string UserName, string MobileNumber, string Openid, string Unionid)
        {
            if (HttpContext.Current.Session["UserSession"] == null)
            {
                UserSession us = new UserSession();
                us.UserId = UserId;
                us.UserName = UserName;
                us.MobileNumber = MobileNumber;
                us.Openid = Openid;
                us.Unionid = Unionid;
                HttpContext.Current.Session.Add("UserSession", us);
            }
            else
            {
                UserSession us = HttpContext.Current.Session["UserSession"] as UserSession;
                if (us == null) us = new UserSession();
                us.UserId = UserId;
                us.UserName = UserName;
                us.MobileNumber = MobileNumber;
                us.Openid = Openid;
                us.Unionid = Unionid;
                HttpContext.Current.Session["UserSession"] = us;
            }
        }

        /// <summary>
        /// 获取当前登录的用户信息
        /// </summary>
        /// </summary>
        /// <returns></returns>
        public static UserSession GetUserInfo()
        {
            UserSession us = HttpContext.Current.Session["UserSession"] as UserSession;
            if (us == null) us = new UserSession();
            return us;
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