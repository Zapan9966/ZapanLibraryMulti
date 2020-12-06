using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace ZapanControls.Libraries
{
    public sealed class TaskCompletionNotifier<TResult> : INotifyPropertyChanged
    {
        public TaskCompletionNotifier(Task<TResult> task)
        {
            Task = task;
            if (task != null && !task.IsCompleted)
            {
                var scheduler = (SynchronizationContext.Current == null) ? TaskScheduler.Current : TaskScheduler.FromCurrentSynchronizationContext();
                task.ContinueWith(t =>
                {
                    var propertyChanged = PropertyChanged;
                    if (propertyChanged != null)
                    {
                        propertyChanged(this, new PropertyChangedEventArgs(nameof(IsCompleted)));
                        if (t.IsCanceled)
                        {
                            propertyChanged(this, new PropertyChangedEventArgs(nameof(IsCanceled)));
                        }
                        else if (t.IsFaulted)
                        {
                            propertyChanged(this, new PropertyChangedEventArgs(nameof(IsFaulted)));
                            propertyChanged(this, new PropertyChangedEventArgs("ErrorMessage"));
                        }
                        else
                        {
                            propertyChanged(this, new PropertyChangedEventArgs(nameof(IsSuccessfullyCompleted)));
                            propertyChanged(this, new PropertyChangedEventArgs(nameof(Result)));
                        }
                    }
                },
                CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously,
                scheduler);
            }
            else
            {
                var propertyChanged = PropertyChanged;
                propertyChanged(this, new PropertyChangedEventArgs(nameof(IsCompleted)));
            }
        }

        // Gets the task being watched. This property never changes and is never <c>null</c>.
        public Task<TResult> Task { get; private set; }

        // Gets the result of the task. Returns the default value of TResult if the task has not completed successfully.
        public TResult Result { get { return (Task.Status == TaskStatus.RanToCompletion) ? Task.Result : default; } }

        // Gets whether the task has completed.
        public bool IsCompleted { get { return Task.IsCompleted; } }

        // Gets whether the task has completed successfully.
        public bool IsSuccessfullyCompleted { get { return Task.Status == TaskStatus.RanToCompletion; } }

        // Gets whether the task has been canceled.
        public bool IsCanceled { get { return Task.IsCanceled; } }

        // Gets whether the task has faulted.
        public bool IsFaulted { get { return Task.IsFaulted; } }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
