using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApliuTools
{
    public class FunctionHelper
    {
        /// <summary>
        /// 以指定的时间 异步 执行有一个参数且有返回值的任务, 时间到达后, 完成或超时则返回结果（任务仍会继续执行）
        /// </summary>
        /// <typeparam name="EveryType">返回结果类型</typeparam>
        /// <param name="taskAction">待执行的任务</param>
        /// <param name="paramsObj">任务的参数</param>
        /// <param name="timeSpan">时间间隔</param>
        /// <param name="callbackAction">执行完成后的回调方法 返回参数为是否执行成功</param>
        /// <param name="throwException">超时是否抛出异常</param>
        /// <returns></returns>
        public static async Task<EveryType> RunTaskWithTimeoutAsync<EveryType>(Func<Object, EveryType> taskAction, Object paramsObj, TimeSpan timeSpan, Action<Boolean> callbackAction, Boolean throwException)
        {
            Boolean isCompleted = false;
            EveryType everyType = RunTaskWithTimeout<EveryType>(taskAction, paramsObj, timeSpan, throwException, out isCompleted);
            callbackAction.Invoke(isCompleted);
            return everyType;
        }

        /// <summary>
        /// 以指定的时间 同步 执行有一个参数且有返回值的任务, 时间到达后, 完成或超时则返回结果（任务仍会继续执行）
        /// </summary>
        /// <typeparam name="EveryType">返回结果类型</typeparam>
        /// <param name="taskAction">待执行的任务</param>
        /// <param name="paramsObj">任务的参数</param>
        /// <param name="timeSpan">时间间隔</param>
        /// <param name="throwException">超时是否抛出异常</param>
        /// <param name="isCompleted">是否执行成功</param>
        /// <returns></returns>
        public static EveryType RunTaskWithTimeout<EveryType>(Func<Object, EveryType> taskAction, Object paramsObj, TimeSpan timeSpan, Boolean throwException, out Boolean isCompleted)
        {
            isCompleted = false;
            try
            {
                if (taskAction == null) return default(EveryType);
                if (timeSpan == null) timeSpan = TimeSpan.FromSeconds(0);

                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                CancellationToken cancellationToken = cancellationTokenSource.Token;

                Task<EveryType> backgroundTask = Task.Factory.StartNew(taskAction, paramsObj, cancellationToken);
                isCompleted = backgroundTask.Wait(timeSpan);

                //是否执行完成
                if (isCompleted)
                {
                    EveryType everyType = backgroundTask.Result;
                    return everyType;
                }
                else
                {
                    cancellationTokenSource.Cancel();
                    //cancellationToken.ThrowIfCancellationRequested();//仅当取消之后执行该操作，才会backgroundTask.IsCanceled是True

                    if (throwException) throw new Exception(taskAction.Method.ToString() + " 任务执行超时");

                    return default(EveryType);
                }
            }
            catch (Exception ex)
            {
                //发生异常
                if (throwException) throw ex;
                return default(EveryType);
            }
        }

        /// <summary>
        /// 以指定的时间 异步 执行任务, 时间到达后, 完成或超时则返回结果（任务仍会继续执行）
        /// </summary>
        /// <typeparam name="EveryType">返回结果类型</typeparam>
        /// <param name="taskAction">待执行的任务</param>
        /// <param name="paramsObj">任务的参数</param>
        /// <param name="timeSpan">时间间隔</param>
        /// <param name="callbackAction">执行完成后的回调方法 返回参数为是否执行成功</param>
        /// <param name="throwException">超时是否抛出异常</param>
        /// <returns></returns>
        public static async void RunTaskWithTimeoutAsync(Action taskAction, TimeSpan timeSpan, Action<Boolean> callbackAction, Boolean throwException)
        {
            Boolean IsCompleted = RunTaskWithTimeout(taskAction, timeSpan, throwException);
            callbackAction.Invoke(IsCompleted);
        }

        /// <summary>
        /// 以指定的时间 同步 执行无参且无返回值的任务, 时间到达后, 完成或超时则返回结果（任务仍会继续执行）
        /// </summary>
        /// <param name="taskAction">待执行的任务</param>
        /// <param name="timeSpan">时间间隔</param>
        /// <param name="throwException">超时是否抛出异常</param>
        /// <returns>是否正常执行完成</returns>
        public static Boolean RunTaskWithTimeout(Action taskAction, TimeSpan timeSpan, Boolean throwException)
        {
            bool IsCompleted = false;
            try
            {
                if (taskAction == null) return IsCompleted;
                if (timeSpan == null) timeSpan = TimeSpan.FromSeconds(0);

                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                CancellationToken cancellationToken = cancellationTokenSource.Token;

                Task backgroundTask = Task.Factory.StartNew(taskAction, cancellationToken);
                IsCompleted = backgroundTask.Wait(timeSpan);

                //是否执行完成
                if (!IsCompleted)
                {
                    cancellationTokenSource.Cancel();
                    //cancellationToken.ThrowIfCancellationRequested();//仅当取消之后执行该操作，才会backgroundTask.IsCanceled是True

                    if (throwException)
                    {
                        throw new Exception(taskAction.Method.ToString() + " 任务执行超时");
                    }
                }
            }
            catch (Exception ex)
            {
                if (throwException) throw ex;
            }
            return IsCompleted;
        }

        /// <summary>
        /// 以指定的时间 异步 执行有一个参数且有返回值的任务, 时间到达后, 完成或超时则返回结果（任务停止执行）
        /// </summary>
        /// <typeparam name="EveryType">返回结果类型</typeparam>
        /// <param name="taskAction">待执行的任务</param>
        /// <param name="paramsObj">任务的参数</param>
        /// <param name="timeSpan">时间间隔</param>
        /// <param name="callbackAction">执行完成后的回调方法 返回参数为是否执行成功</param>
        /// <param name="throwException">超时是否抛出异常</param>
        /// <returns></returns>
        public static async Task<EveryType> RunThreadWithTimeoutAsync<EveryType>(Func<Object, EveryType> taskAction, Object paramsObj, TimeSpan timeSpan, Action<Boolean> callbackAction, Boolean throwException)
        {
            Boolean isCompleted = false;
            EveryType everyType = RunThreadWithTimeout<EveryType>(taskAction, paramsObj, timeSpan, throwException, out isCompleted);
            callbackAction.Invoke(isCompleted);
            return everyType;
        }

        /// <summary>
        /// 以指定的时间 同步 执行有一个参数且有返回值的任务, 时间到达后, 完成或超时则返回结果（任务停止执行）
        /// </summary>
        /// <typeparam name="EveryType">返回结果类型</typeparam>
        /// <param name="taskAction">待执行的任务</param>
        /// <param name="paramsObj">任务的参数</param>
        /// <param name="timeSpan">时间间隔</param>
        /// <param name="throwException">超时是否抛出异常</param>
        /// <param name="isCompleted">是否执行成功</param>
        /// <returns></returns>
        public static EveryType RunThreadWithTimeout<EveryType>(Func<Object, EveryType> taskAction, Object paramsObj, TimeSpan timeSpan, Boolean throwException, out Boolean isCompleted)
        {
            isCompleted = false;
            try
            {
                if (taskAction == null) return default(EveryType);
                if (timeSpan == null) timeSpan = TimeSpan.FromSeconds(0);

                ThreadResult<EveryType> threadParams = new ThreadResult<EveryType>(taskAction, paramsObj);
                Thread thread = new Thread(new ThreadStart(threadParams.RunThread));
                thread.IsBackground = true;
                thread.Start();
                Thread.Sleep(timeSpan);
                isCompleted = !thread.IsAlive;
                thread.Abort();

                //是否执行完成
                if (isCompleted)
                {
                    EveryType everyType = threadParams.Result;
                    return everyType;
                }
                else
                {
                    if (throwException) throw new Exception(taskAction.Method.ToString() + " 任务执行超时");
                    return default(EveryType);
                }
            }
            catch (Exception ex)
            {
                //发生异常
                if (throwException) throw ex;
                return default(EveryType);
            }
        }

        /// <summary>
        /// 以指定的时间 异步 执行任务, 时间到达后, 完成或超时则返回结果（任务停止执行）
        /// </summary>
        /// <typeparam name="EveryType">返回结果类型</typeparam>
        /// <param name="taskAction">待执行的任务</param>
        /// <param name="paramsObj">任务的参数</param>
        /// <param name="timeSpan">时间间隔</param>
        /// <param name="callbackAction">执行完成后的回调方法 返回参数为是否执行成功</param>
        /// <param name="throwException">超时是否抛出异常</param>
        /// <returns></returns>
        public static async void RunThreadWithTimeoutAsync(Action taskAction, TimeSpan timeSpan, Action<Boolean> callbackAction, Boolean throwException)
        {
            Boolean IsCompleted = RunThreadWithTimeout(taskAction, timeSpan, throwException);
            callbackAction.Invoke(IsCompleted);
        }

        /// <summary>
        /// 以指定的时间 同步 执行无参且无返回值的任务, 时间到达后, 完成或超时则返回结果（任务停止执行）
        /// </summary>
        /// <param name="taskAction">待执行的任务</param>
        /// <param name="timeSpan">时间间隔</param>
        /// <param name="throwException">超时是否抛出异常</param>
        /// <returns>是否正常执行完成</returns>
        public static Boolean RunThreadWithTimeout(Action taskAction, TimeSpan timeSpan, Boolean throwException)
        {
            bool IsCompleted = false;
            try
            {
                if (taskAction == null) return IsCompleted;
                if (timeSpan == null) timeSpan = TimeSpan.FromSeconds(0);

                Thread thread = new Thread(new ThreadStart(taskAction));
                thread.IsBackground = true;
                thread.Start();
                Thread.Sleep(timeSpan);
                IsCompleted = !thread.IsAlive;
                thread.Abort();
            }
            catch (Exception ex)
            {
                if (throwException) throw ex;
            }
            return IsCompleted;
        }

        /// <summary>
        /// 异步等待指定时间后执行有一个参数且无返回值的任务
        /// </summary>
        /// <param name="action">待执行的任务</param>
        /// <param name="paramsObj">任务的参数</param>
        /// <param name="timeSpan">等待时间</param>
        /// <param name="callbackAction">执行完成后的回调方法</param>
        /// <param name="throwException">是否抛出异常</param>
        /// <returns></returns>
        public static async Task RunTaskTimingAsync(Action<Object> action, Object paramsObj, TimeSpan timeSpan, Action<Boolean> callbackAction, Boolean throwException)
        {
            bool IsCompleted = false;
            if (action == null)
            {
                return;
            }

            try
            {
                if (timeSpan != null) await Task.Delay(timeSpan);
                action.Invoke(paramsObj);
                IsCompleted = true;
            }
            catch (Exception ex)
            {
                if (throwException) throw ex;
            }
            if (callbackAction != null) callbackAction.Invoke(IsCompleted);
        }

        /// <summary>
        /// 异步等待指定时间后执行无参且无返回值的任务
        /// </summary>
        /// <param name="action">待执行的任务</param>
        /// <param name="timeSpan">等待时间</param>
        /// <param name="callbackAction">执行完成后的回调方法</param>
        /// <param name="throwException">是否抛出异常</param>
        /// <returns></returns>
        public static async Task RunTaskTimingAsync(Action action, TimeSpan timeSpan, Action<Boolean> callbackAction, Boolean throwException)
        {
            bool IsCompleted = false;
            if (action == null)
            {
                return;
            }

            try
            {
                if (timeSpan != null) await Task.Delay(timeSpan);
                action.Invoke();
                IsCompleted = true;
            }
            catch (Exception ex)
            {
                if (throwException) throw ex;
            }
            if (callbackAction != null) callbackAction.Invoke(IsCompleted);
        }
    }
}