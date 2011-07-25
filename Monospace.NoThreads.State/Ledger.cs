using System;
using System.Collections.Generic;
using System.Linq;

namespace Monospace.NoThreads.State {
    public class Ledger {
        private readonly List<Entry> _entries = new List<Entry>();

        public void Credit(decimal amount, string description) {
            _entries.Add(new Entry(true, amount, description));
        }

        public void Debit(decimal amount, string description) {
            _entries.Add(new Entry(false, amount, description));
        }

        public Decimal Balance { get { return _entries.Select(x => x.Credit ? x.Amount : -1*x.Amount).Sum(); }}

        public IEnumerable<Entry> Entries { get { return _entries.ToArray(); } }

        public class Entry {
            public readonly bool Credit;
            public readonly decimal Amount;
            public readonly string Description;

            public Entry(bool credit, decimal amount, string description) {
                Credit = credit;
                Amount = amount;
                Description = description;
            }
        }
    }
}