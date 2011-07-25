using System;

namespace Monospace.NoThreads.AsyncCoroutines {
    internal class Producer2 {

        public static async void Coroutine(int[,] source, Coordinator<int[]> coordinator) {

            Console.WriteLine("producer started");
            for(var i = 0; i < source.GetLength(0); i++) {

                coordinator.State = new int[source.GetLength(1)];
                for(var j = 0; j < source.GetLength(1); j++) {

                    var item = source[i, j];

                    // awaiting a regular async method while in a coordinator controlled coroutine (not possible with Iterator based coroutines)
                    var exponentiated = await Exponentiator2.AsyncMethod(item);
                    coordinator.State[j] = exponentiated;

                    Console.WriteLine("read {0} from [{1},{2}] and processed to {3}", item, i, j, exponentiated);
                }

                Console.WriteLine("awaiting consumer");
                await coordinator;
            }

            coordinator.State = new int[0];
            Console.WriteLine("producer finished");
        }

    }
}