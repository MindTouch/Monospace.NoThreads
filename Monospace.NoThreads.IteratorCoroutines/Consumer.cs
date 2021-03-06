using System;
using System.Collections;
using System.Collections.Generic;

namespace Monospace.NoThreads.IteratorCoroutines {
class Consumer {
    public static IEnumerator Coroutine(int[,] destination, Coordinator<int[]> coordinator) {
        Console.WriteLine("consumer started");
        int i = 0, j = 0;
        while(true) {
            Console.WriteLine("yielding to producer");
            yield return null;
            if(coordinator.State == null) {
                continue;
            }
            if(coordinator.State.Length == 0) {
                Console.WriteLine("consumer finished, end of input");
                yield break;
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
                if(i != destination.GetLength(0)) {
                    continue;
                }
                Console.WriteLine("consumer finished, output full");
                yield break;
            }
        }
    }
}
}