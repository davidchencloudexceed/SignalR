﻿
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using CommonUtility.Infrastructure;

namespace CommonUtility.TBTask
{
    // Allows serial queuing of Task instances
    // The tasks are not called on the current synchronization context

    public sealed class TaskQueue
    {
        private readonly object _lockObj = new object();
        private Task _lastQueuedTask;
        private volatile bool _drained;
        private readonly int? _maxSize;
        private long _size;

        public TaskQueue()
            : this(TaskAsyncHelper.Empty)
        {
        }

        public TaskQueue(Task initialTask)
        {
            _lastQueuedTask = initialTask;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "This is shared code")]
        public TaskQueue(Task initialTask, int maxSize)
        {
            _lastQueuedTask = initialTask;
            _maxSize = maxSize;
        }




        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "This is shared code.")]
        public IPerformanceCounter QueueSizeCounter { get; set; }
        public Task Enqueue(Func<object, Task> taskFunc, object state)
        {
            // Lock the object for as short amount of time as possible
            lock (_lockObj)
            {
                if (_drained)
                {
                    return _lastQueuedTask;
                }

                if (_maxSize != null)
                {
                    // Increment the size if the queue
                    if (Interlocked.Increment(ref _size) > _maxSize)
                    {
                        Interlocked.Decrement(ref _size);

                        // We failed to enqueue because the size limit was reached
                        return null;
                    }

                }

                var newTask = _lastQueuedTask.Then((n, ns, q) => q.InvokeNext(n, ns), taskFunc, state, this);

                _lastQueuedTask = newTask;
                return newTask;
            }
        }

        private Task InvokeNext(Func<object, Task> next, object nextState)
        {

            return next(nextState).Finally(s => ((TaskQueue)s).Dequeue(), this);
        }

        private void Dequeue()
        {

            if (_maxSize != null)
            {
                // Decrement the number of items left in the queue
                Interlocked.Decrement(ref _size);

            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "This is shared code")]
        public Task Enqueue(Func<Task> taskFunc)
        {
            return Enqueue(state => ((Func<Task>)state).Invoke(), taskFunc);
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "This is shared code")]
        public Task Drain()
        {
            lock (_lockObj)
            {
                _drained = true;

                return _lastQueuedTask;
            }
        }
    }
}
