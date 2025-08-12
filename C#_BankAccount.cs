using System;
using System.Collections.Generic;

namespace task3
{
    internal class Program
    {
        public class bankaccount
        {
            public string Accountnum { get; set; }
            public string Accountholder { get; set; }
            public decimal Balance { get; set; }

            public bankaccount(string accountnum, string accountholder, decimal balance)
            {
                Accountnum = accountnum;
                Accountholder = accountholder;
                Balance = balance;
            }

            public virtual decimal calcinterest()
            {
                return 0;
            }

            public virtual void show()
            {
                Console.WriteLine($"account number: {Accountnum}");
                Console.WriteLine($"account holder: {Accountholder}");
                Console.WriteLine($"balance: {Balance}");
            }
        }

        public class savingaccount : bankaccount
        {
            public decimal Interestrate { get; set; }

            public savingaccount(string accountnum, string accountholder, decimal balance, decimal interestrate)
                : base(accountnum, accountholder, balance)
            {
                Interestrate = interestrate;
            }

            public override decimal calcinterest()
            {
                return Balance * Interestrate / 100;
            }

            public override void show()
            {
                base.show();
                Console.WriteLine($"interest rate: {Interestrate}%");
            }
        }

        public class currentaccount : bankaccount
        {
            public decimal Overdraftlimit { get; set; }

            public currentaccount(string accountnum, string accountholder, decimal balance, decimal overdraftlimit)
                : base(accountnum, accountholder, balance)
            {
                Overdraftlimit = overdraftlimit;
            }

            public override decimal calcinterest()
            {
                return 0;
            }

            public override void show()
            {
                base.show();
                Console.WriteLine($"overdraft limit: {Overdraftlimit}");
            }
        }

        static void Main(string[] args)
        {
            savingaccount s = new savingaccount("SA123", "Nayira", 5000m, 5m);
            currentaccount c = new currentaccount("CA456", "mona", 3000m, 1000m);

            List<bankaccount> accounts = new List<bankaccount> { s, c };

            foreach (var account in accounts)
            {
                account.show();
                Console.WriteLine($"Interest: {account.calcinterest():C}");
                Console.WriteLine(new string('-', 30));
            }
        }
    }
}
