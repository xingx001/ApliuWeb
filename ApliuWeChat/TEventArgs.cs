using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApliuWeChat.Helper
{
    /// <summary>
    /// 自定义回调事件参数
    /// </summary>
    /// <typeparam name="T">泛型类返回</typeparam>
    public class TEventArgs<T> : EventArgs
    {
        public T Result { get; private set; }
        public TEventArgs(T obj)
        {
            this.Result = obj;
        }
    }
}
