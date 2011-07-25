using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Monospace.NoThreads.IteratorCoroutines {
    [TestFixture]
    public class IteratorExample {

        [Test]
        public void Run() {
            Console.WriteLine("creating iterator");

            // does not "call" method, but creates an iterator from the method
            var iterator = Produce();
            Console.WriteLine("moving next");

            // run from start to first yield
            var moved = iterator.MoveNext();
            Console.WriteLine("has data: {0}", moved);
            Console.WriteLine("current: {0}", iterator.Current);
            Console.WriteLine("moving next");

            // run to next yield
            moved = iterator.MoveNext();
            Console.WriteLine("has data: {0}", moved);
            Console.WriteLine("current: {0}", iterator.Current);
            Console.WriteLine("moving next");

            // run to next yield (or exit of method in this case)
            moved = iterator.MoveNext();
            Console.WriteLine("has data: {0}", moved);
        }

        /// <summary>
        /// Enumerator methods that contain at least one yield do not actually return a value of IEnumerator but are converted by the compiler into
        /// a statemachine at call time which does not execute the method until the first call to MoveNext
        /// </summary>
        /// <returns></returns>
        public IEnumerator<int> Produce() {
            Console.WriteLine("entering Produce");
            for(int i = 0; i < 2; i++) {
                Console.WriteLine("Produce yielding {0}", i);
        
                yield return i;
                Console.WriteLine("Produce resuming");
            }
            Console.WriteLine("Produce is done");
        }
    }
}