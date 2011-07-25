using System;
using System.Collections.Generic;

namespace Monospace.NoThreads.IteratorCoroutines {
    public class Coordinator<T> {

        private readonly Queue<IEnumerator<Coordinator<T>>> _coroutines = new Queue<IEnumerator<Coordinator<T>>>();
        
        public Coordinator(IEnumerable<Func<Coordinator<T>, IEnumerator<Coordinator<T>>>> coroutines) {

            // add all coroutines to our queue of coroutines to execute
            foreach(var coroutine in coroutines) {
                _coroutines.Enqueue(coroutine(this));
            }
        }

        public void Execute() {

            // iterate over coroutines
            while(_coroutines.Count > 0) {

                // get next coroutine
                var coroutine = _coroutines.Dequeue();
                if(coroutine.MoveNext()) {

                    // if MoveNext was true, the coroutine expects to be resumed, so we put the coroutine back in the queue to
                    // call MoveNext again once we've given the other coroutines a chance
                    _coroutines.Enqueue(coroutine);
                }
            }
        }

        public T State { get; set; }
    }
}