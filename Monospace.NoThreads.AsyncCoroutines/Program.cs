using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Monospace.NoThreads.AsyncCoroutines {
    
    [TestFixture]
    public class Program {
      
        static void Main(string[] args) {
            var program = new Program();
            Run("Transpose", program.Transpose);
            Run("Transpose and Exponentiate - I", program.Transpose_and_exponentiate1);
            Run("Transpose and Exponentiate - II", program.Transpose_and_exponentiate2);
        }

        [Test]
        public void Transpose() {
            var source = new int[3, 4] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 } };
            var destination = new int[6, 2];
            var coordinator = new Coordinator<int[]>(
                new Func<Coordinator<int[]>, Action>[] {
                    c => () => Consumer.Coroutine(destination, c),
                    c => () => Producer.Coroutine(source, c),
                }
            );
            Print("input", source);
            coordinator.Execute();
            Print("output", destination);
        }

        [Test]
        public void Transpose_and_exponentiate1() {
            var source = new int[3, 4] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 } };
            var destination = new int[6, 2];
            var coordinator = new Coordinator<int[]>(
                new Func<Coordinator<int[]>, Action>[] {
                    c => () => Consumer.Coroutine(destination, c),
                    c => () => Producer.Coroutine(source, c),
                    c => () => Exponentiator.Coroutine(c)
                }
            );
            Print("input", source);
            coordinator.Execute();
            Print("output", destination);
        }

        [Test]
        public void Transpose_and_exponentiate2() {
            var source = new int[3, 4] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 } };
            var destination = new int[6, 2];
            var coordinator = new Coordinator<int[]>(
                new Func<Coordinator<int[]>, Action>[] {
                    c => () => Consumer.Coroutine(destination, c),
                    c => () => Producer2.Coroutine(source, c),
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
