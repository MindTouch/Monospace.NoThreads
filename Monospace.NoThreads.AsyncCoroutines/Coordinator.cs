using System;
using System.Collections.Generic;

namespace Monospace.NoThreads.AsyncCoroutines {

    public class Coordinator<T> {
        private readonly Queue<Action> _actions = new Queue<Action>();

        public Coordinator(IEnumerable<Func<Coordinator<T>, Action>> coroutines) {

            // prime continuation queue with each coroutine's entrypoint
            foreach(var coroutine in coroutines) {
                _actions.Enqueue(coroutine(this));
            }
        }

        public void Execute() {

            // iterate over continuations
            while(_actions.Count > 0) {
                _actions.Dequeue().Invoke();
            }
        }

        // the shared state for all coroutines
        public T State { get; set; }

        // required to make Coordinator "await"able
        public Coordinator<T> GetAwaiter() { return this; }

        // has the awaitable finished?
        public bool IsCompleted { get { return false; } }

        // if it hasn't finished here's the continuation to pick up execution
        public void OnCompleted(Action continuation) {

            // put the continuation into the queue, so we can pick it back up when we've run the other coroutine continuations in line
            _actions.Enqueue(continuation);
        }

        // the value of the awaitable (void for the coordinator)
        public void GetResult() { }
    }

}