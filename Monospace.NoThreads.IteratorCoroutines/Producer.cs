using System;
using System.Collections;
using System.Collections.Generic;

namespace Monospace.NoThreads.IteratorCoroutines {
    class Producer {
        public static IEnumerator Coroutine(int[,] source, Coordinator<int[]> coordinator) {
            Console.WriteLine("producer started");
            for(var i = 0; i < source.GetLength(0); i++) {
                coordinator.State = new int[source.GetLength(1)];
                for(var j = 0; j < source.GetLength(1); j++) {
                    coordinator.State[j] = source[i, j];
                    Console.WriteLine("read {0} from [{1},{2}]", source[i, j], i, j);
                }
                Console.WriteLine("yielding to consumer");
                yield return null;
            }
            coordinator.State = new int[0];
            Console.WriteLine("producer finished");
        }
    }
}
