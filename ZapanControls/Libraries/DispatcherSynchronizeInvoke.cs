using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Threading;

namespace ZapanControls.Libraries
{
    // Source: https://stackoverflow.com/questions/50161741/how-to-use-the-isynchronizeinvoke-interface-in-wpf
    public class DispatcherSynchronizeInvoke : ISynchronizeInvoke
    {
        private readonly Dispatcher _dispatcher;

        public DispatcherSynchronizeInvoke(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public IAsyncResult BeginInvoke(Delegate method, object[] args)
        {
            // Obtaining a DispatcherOperation instance
            // and wrapping it with our proxy class
            return new DispatcherAsyncResult(
                _dispatcher.BeginInvoke(method, DispatcherPriority.Normal, args));
        }

        public object EndInvoke(IAsyncResult result)
        {
            DispatcherAsyncResult dispatcherResult = result as DispatcherAsyncResult;
            dispatcherResult?.Operation.Wait();
            return dispatcherResult.Operation.Result;
        }

        public object Invoke(Delegate method, object[] args)
            => _dispatcher.Invoke(method, DispatcherPriority.Normal, args);

        public bool InvokeRequired => !_dispatcher.CheckAccess();

        // We also could use the DispatcherOperation.Task directly
        private class DispatcherAsyncResult : IAsyncResult
        {
            private readonly IAsyncResult result;

            public DispatcherAsyncResult(DispatcherOperation operation)
            {
                Operation = operation;
                result = operation.Task;
            }

            public DispatcherOperation Operation { get; }

            public bool IsCompleted => result.IsCompleted;
            public WaitHandle AsyncWaitHandle => result.AsyncWaitHandle;
            public object AsyncState => result.AsyncState;
            public bool CompletedSynchronously => result.CompletedSynchronously;
        }
    }
}
