using System;
using NUnit.Framework;

namespace Monospace.NoThreads.State {
    public class SharingWithLocks {

        public static void Run(Ledger ledger) {

            // Making a deposit
            decimal balance = 0;
            lock(ledger) {
                ledger.Credit(new Random().Next(5, 15), "deposit");
                balance = ledger.Balance;
            }
            Console.WriteLine("Balance: {0:0.00}", balance);

            // Withdrawing spending money depending on balance and checking resulting balance "atomically"
            decimal spendingMoney = 0;
            lock(ledger) {
                if(ledger.Balance > 10) {
                    spendingMoney = 10;
                } else {
                    spendingMoney = 5;
                }
                ledger.Debit(spendingMoney, "wastin money");
                balance = ledger.Balance;
            }
            Console.WriteLine("withdrew: {0:0.00}", spendingMoney);
            Console.WriteLine("balance: {0:0.00}", balance);
        }
    }
}
