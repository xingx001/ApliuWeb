using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApliuWeb
{
    public class SessionHelper
    {
        /// <summary>
        /// 设置Session的值
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool SetSessionValue(string Name, object Value)
        {
            bool Result = false;
            try
            {
                if (HttpContext.Current.Session[Name] == null) HttpContext.Current.Session.Add(Name, Value);
                else HttpContext.Current.Session[Name] = Value;
                Result = true;
            }
            catch (Exception ex)
            {
                Result = false;
            }
            return Result;
        }

        /// <summary>
        /// 获取Session的值
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static object GetSessionValue(string Name)
        {
            object Result = null;
            try
            {
                if (HttpContext.Current.Session[Name] != null) Result = HttpContext.Current.Session[Name];
            }
            catch (Exception ex)
            {
                Result = null;
            }
            return Result;
        }

        /// <summary>
        /// 删除Session
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static bool DeleteSession(string Name)
        {
            bool Result = false;
            try
            {
                HttpContext.Current.Session.Remove(Name);
                Result = true;
            }
            catch (Exception ex)
            {
                Result = false;
            }
            return Result;
        }
    }
}