using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monospace.NoThreads.AsyncCoroutines {
    class Producer {

        public static async void Coroutine(int[,] source, Coordinator<int[]> coordinator) {

            Console.WriteLine("producer started");
            for(var i = 0; i < source.GetLength(0); i++) {

                coordinator.State = new int[source.GetLength(1)];
                for(var j = 0; j < source.GetLength(1); j++) {

                    var item = source[i, j];
                    coordinator.State[j] = item;

                    Console.WriteLine("read {0} from [{1},{2}]", item, i, j);
                }

                Console.WriteLine("awaiting consumer");
                await coordinator;
            }

            coordinator.State = new int[0];
            Console.WriteLine("producer finished");
        }
    }
}
