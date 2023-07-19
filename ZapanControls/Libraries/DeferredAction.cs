using System;
using System.Threading;
using System.Windows;

namespace ZapanControls.Libraries
{
    // Source : https://www.codeproject.com/Articles/32426/Deferring-ListCollectionView-filter-updates-for-a

    /// <summary>
    /// Represents a timer which performs an action on the UI thread when time elapses.  Rescheduling is supported.
    /// </summary>
    public sealed class DeferredAction : IDisposable
    {
        private readonly Timer timer;
        private bool _disposed;
   
        /// <summary>
        /// Creates a new DeferredAction.
        /// </summary>
        /// <param name="action">
        /// The action that will be deferred.  It is not performed until after <see cref="Defer"/> is called.
        /// </param>
        public static DeferredAction Create(Action action)
        {
            return action != null 
                ? new DeferredAction(action) 
                : throw new ArgumentNullException(nameof(action));
        }

        private DeferredAction(Action action)
        {
            timer = new Timer(new TimerCallback(delegate
            {
                Application.Current.Dispatcher.BeginInvoke(action, System.Windows.Threading.DispatcherPriority.Background);
            }));
        }
   
        /// <summary>
        /// Defers performing the action until after time elapses.  Repeated calls will reschedule the action
        /// if it has not already been performed.
        /// </summary>
        /// <param name="delay">
        /// The amount of time to wait before performing the action.
        /// </param>
        public void Defer(TimeSpan delay)
        {
            // Fire action when time elapses (with no subsequent calls).
            timer.Change(delay, TimeSpan.FromMilliseconds(-1));
        }
   
        #region IDisposable Members
   
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                timer?.Dispose();
            }

            _disposed = true;
        }
   
        #endregion
    }
}
