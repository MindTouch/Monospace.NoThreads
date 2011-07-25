using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Monospace.NoThreads.State {
    public class AsyncAdapter<T> : IAsyncAdapter<T> {
        private readonly T _instance;
        private readonly Queue<Task> _tasks = new Queue<Task>();
        private object _reserved;

        public AsyncAdapter(T instance) {
            _instance = instance;
        }

        public Task<TValue> Call<TValue>(Func<T, TValue> visitor) {
            if(ReserveExecution()) {
                var completion = new TaskCompletionSource<TValue>();
                try {
                    completion.SetResult(visitor(_instance));
                } catch(Exception e) {
                    completion.SetException(e);
                } finally {
                    _reserved = null;
                }
                Poke();
                return completion.Task;
            }
            var task = new Task<TValue>(() => visitor(_instance));
            Schedule(task);
            return task;
        }

        private void Schedule(Task task) {
            lock(_tasks) {
                _tasks.Enqueue(task);
            }
            Poke();
        }

        private void Poke() {
            if(_reserved != null) {
                return;
            } 
            ThreadPool.QueueUserWorkItem(c => {
                if(!ReserveExecution()) {
                    return;
                }
                try {
                    while(true) {
                        Task t;
                        lock(_tasks) {
                            if(!_tasks.Any()) {
                                return;
                            }
                            t = _tasks.Dequeue();
                        }
                        t.RunSynchronously();
                    }
                } finally {
                    _reserved = null;
                }
            });
        }

        public Task Call(Action<T> visitor) {
            if(ReserveExecution()) {
                var completion = new TaskCompletionSource<object>();
                try {
                    visitor(_instance);
                    completion.SetResult(null);
                } catch(Exception e) {
                    completion.SetException(e);
                } finally {
                    _reserved = null;
                }
                Poke();
                return completion.Task;
            }
            var task = new Task(() => visitor(_instance));
            Schedule(task);
            return task;
        }

        private bool ReserveExecution() {
            if(_reserved == null) {
                var reservation = new object();
                var reserved = Interlocked.CompareExchange(ref _reserved, reservation, null);
                if(reserved == null) {
                    return true;
                }
            }
            return false;
        }
    }
}
