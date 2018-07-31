using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApliuTools
{
    /// <summary>
    /// 提供线程异步执行时，保存返回结果的封装类
    /// </summary>
    /// <typeparam name="EveryType"></typeparam>
    public class ThreadResult<EveryType>
    {
        private Func<Object, EveryType> TaskAction;
        private Object ParamsObj;
        /// <summary>
        /// 任务执行的结果，默认值 default(EveryType)
        /// </summary>
        public EveryType Result = default(EveryType);
        /// <summary>
        /// 创建 线程异步执行时，保存返回结果的封装类
        /// </summary>
        /// <param name="taskAction">待执行的任务</param>
        /// <param name="paramsObj">任务的参数</param>
        public ThreadResult(Func<Object, EveryType> taskAction, Object paramsObj)
        {
            this.TaskAction = taskAction;
            this.ParamsObj = paramsObj;
        }
        /// <summary>
        /// 开始执行任务，并将结果保存到Result属性中
        /// </summary>
        public void RunThread()
        {
            Result = TaskAction.Invoke(ParamsObj);
        }
    }
}
