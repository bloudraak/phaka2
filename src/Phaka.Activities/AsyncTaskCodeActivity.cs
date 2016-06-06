using System;
using System.Activities;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace Phaka.Activities
{
    public abstract class AsyncTaskCodeActivity<T> : AsyncCodeActivity<T>
    {
        protected sealed override IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback,
            object state)
        {
            var task = ExecuteAsync(context);
            var tcs = new TaskCompletionSource<T>(state);
            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    if (t.Exception != null) tcs.TrySetException(t.Exception.InnerExceptions);
                }
                else if (t.IsCanceled)
                    tcs.TrySetCanceled();
                else
                    tcs.TrySetResult(t.Result);

                callback?.Invoke(tcs.Task);
            });

            return tcs.Task;
        }

        protected sealed override T EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
        {
            var task = (Task<T>) result;
            try
            {
                return task.Result;
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        protected abstract Task<T> ExecuteAsync(AsyncCodeActivityContext context);
    }

    public abstract class AsyncTaskCodeActivity : AsyncCodeActivity
    {
        protected sealed override IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback,
            object state)
        {
            var task = ExecuteAsync(context);
            var tcs = new TaskCompletionSource<object>(state);
            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    if (t.Exception != null) tcs.TrySetException(t.Exception.InnerExceptions);
                }
                else if (t.IsCanceled)
                    tcs.TrySetCanceled();
                else
                    tcs.TrySetResult(null);

                callback?.Invoke(tcs.Task);
            });

            return tcs.Task;
        }

        protected sealed override void EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
        {
            var task = (Task)result;
            try
            {
                task.Wait();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        protected abstract Task ExecuteAsync(AsyncCodeActivityContext context);
    }
}