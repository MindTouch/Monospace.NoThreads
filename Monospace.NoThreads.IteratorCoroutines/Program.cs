using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NUnit.Framework;

namespace Monospace.NoThreads.IteratorCoroutines {

    [TestFixture]
    public class Program {

        static void Main(string[] args) {
            var program = new Program();
            Run("Iterator example", new IteratorExample().Run);
            Run("Transpose", program.Transpose);
            Run("Transpose and Exponentiate", program.Transpose_and_exponentiate);
        }

        [Test]
        public void Transpose() {
            var source = new int[3, 4] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 } };
            var destination = new int[6, 2];
            var coordinator = new Coordinator<int[]>(
                new Func<Coordinator<int[]>, IEnumerator<Coordinator<int[]>>>[] {

                    // Curry coroutine into common "shape"
                    c => Consumer.Coroutine(destination, c),
                    c => Producer.Coroutine(source, c),
                }
            );
            Print("input", source);
            coordinator.Execute();
            Print("output", destination);
        }

        [Test]
        public void Transpose_and_exponentiate() {
            var source = new int[3, 4] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 } };
            var destination = new int[6, 2];
            var coordinator = new Coordinator<int[]>(
                new Func<Coordinator<int[]>, IEnumerator<Coordinator<int[]>>>[] {

                    // Curry coroutine into common "shape"
                    c => Consumer.Coroutine(destination, c),
                    c => Producer.Coroutine(source, c),
                    Exponentiator.Coroutine
                }
            );
            Print("input", source);
            coordinator.Execute();
            Print("output", destination);
        }

        private static void Run(string title, Action action) {
            Console.WriteLine("=== {0} ===", title);
            action();
        }

        private void Print(string title, int[,] array) {
            Console.WriteLine("{0}:", title);
            Console.WriteLine("[");
            for(var i = 0; i < array.GetLength(0); i++) {
                Console.Write("  [ ");
                for(var j = 0; j < array.GetLength(1); j++) {
                    Console.Write("{0},", array[i, j]);
                }
                Console.WriteLine(" ]");
            }
            Console.WriteLine("]");
        }
    }
}
