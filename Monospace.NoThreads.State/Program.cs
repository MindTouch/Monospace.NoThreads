using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Monospace.NoThreads.State {

    [TestFixture]
    public class Program {
        static void Main(string[] args) {
            var program = new Program();
            Run("Sharing with locks",program.Sharing_with_locks);
            Run("Sharing with async",program.Sharing_with_async);
            var asyncMethod = new AsyncMethodExample();
            Run("Async Method Example using Task<T>",asyncMethod.Calling_Task_async_method);
            Run("Async Method Example using Result<T>",asyncMethod.Calling_Result_async_method);
        }


        [Test]
        public void Sharing_with_locks() {
            var ledger = new Ledger();
            SharingWithLocks.Run(ledger);
        }

        [Test]
        public void Sharing_with_async() {
            var ledger = new Ledger();
            var adapter = new AsyncAdapter<Ledger>(ledger);
            SharingWithAsync.Run(adapter).Wait();
        }

        private static void Run(string title, Action action) {
            Console.WriteLine("=== {0} ===", title);
            action();
        }

    }
}
