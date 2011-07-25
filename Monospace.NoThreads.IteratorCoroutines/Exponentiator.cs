using System;
using System.Collections.Generic;

namespace Monospace.NoThreads.IteratorCoroutines {
    class Exponentiator {
        public static IEnumerator<Coordinator<int[]>> Coroutine(Coordinator<int[]> coordinator) {
            while(true) {
                if(coordinator.State == null) {
                    continue;
                }
                if(coordinator.State.Length == 0) {
                    Console.WriteLine("consumer finished, end of input");
                    yield break;
                }
                for(int i = 0; i < coordinator.State.Length; i++) {
                    var item = coordinator.State[i];
                    coordinator.State[i] = item * item;
                }
                yield return coordinator;
            }
        }
    }
}