using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monospace.NoThreads.State {
    public class SharingWithAsync {

        public static async Task Run(IAsyncAdapter<Ledger> adapter) {

            // Making a deposit
            decimal balance = await adapter.Call(ledger => {
                ledger.Credit(new Random().Next(5, 15), "deposit");
                return ledger.Balance;
            });
            Console.WriteLine("Balance: {0:0.00}", balance);

            // Withdrawing spending money depending on balance and checking resulting balance "atomically"
            // can return or capture variables (but capturing the ledger would defeat the puporse of the adapter)
            decimal spendingMoney = 0;
            await adapter.Call(ledger => {
                if(ledger.Balance > 10) {
                    spendingMoney = 10;
                } else {
                    spendingMoney = 5;
                }
                ledger.Debit(spendingMoney, "wastin money");
                balance = ledger.Balance;
            });
            Console.WriteLine("withdrew: {0:0.00}", spendingMoney);
            Console.WriteLine("balance: {0:0.00}", balance);
        }
   }
}
