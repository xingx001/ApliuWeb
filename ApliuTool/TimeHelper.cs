using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApliuTools
{
    public class TimeHelper
    {
        /// <summary>
        /// 获取系统当前时间戳 Unix
        /// </summary>
        /// <returns></returns>
        public static long getCurrentUnixTime()
        {
            return (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        /// <summary>
        /// 获取指定时间戳 Unix
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long getUnixTime(DateTime dt)
        {
            return (long)(dt.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }
}
