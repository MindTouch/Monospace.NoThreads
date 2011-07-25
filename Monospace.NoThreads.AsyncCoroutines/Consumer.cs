using System;

namespace Monospace.NoThreads.AsyncCoroutines {
    class Consumer {
        public static async void Coroutine(int[,] destination, Coordinator<int[]> coordinator) {

            Console.WriteLine("consumer started");
            int i = 0, j = 0;
            while(true) {

                Console.WriteLine("awaiting producer");
                await coordinator;
                if(coordinator.State == null) {
                    continue;
                }
                if(coordinator.State.Length == 0) {

                    Console.WriteLine("consumer finished, end of input");
                    return;
                }
                foreach(var item in coordinator.State) {
                    destination[i, j] = item;

                    Console.WriteLine("wrote {0} to [{1},{2}]", item, i, j);
                    j++;
                    if(j != destination.GetLength(1)) {
                        continue;
                    }
                    j = 0;
                    i++;
                    if(i == destination.GetLength(0)) {

                        Console.WriteLine("consumer finished, output full");
                        return;
                    }
                }
            }
        }
    }
}