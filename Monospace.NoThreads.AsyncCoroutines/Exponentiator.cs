using System;

namespace Monospace.NoThreads.AsyncCoroutines {
    class Exponentiator {

        public static async void Coroutine(Coordinator<int[]> coordinator) {
            while(true) {
                if(coordinator.State == null) {
                    continue;
                }
                if(coordinator.State.Length == 0) {
                    Console.WriteLine("consumer finished, end of input");
                    return;
                }
                for(int i = 0; i < coordinator.State.Length; i++) {
                    var item = coordinator.State[i];
                    coordinator.State[i] = item * item;
                }
                await coordinator;
            }
        }
    }
}