using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BankSystemApp
{
    // Utility class for validated input
    public static class InputHelper
    {
        public static string ReadNonEmptyString(string prompt)
        {
            string value = "";
            while (string.IsNullOrEmpty(value))
            {
                Console.Write(prompt);
                value = Console.ReadLine();

                if (string.IsNullOrEmpty(value))
                {
                    Console.WriteLine("Invalid. Cannot be empty.");
                }
            }
            return value;
        }


        public static string ReadName(string prompt)
        {
            string name;
            do
            {
                Console.Write(prompt);
                name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Name cannot be empty. Try again.");
                    name = null;
                }
            } while (name == null);

            return name;
        }


        public static string ReadNationalId(string prompt)
        {
            string nid;
            do
            {
                Console.Write(prompt);
                nid = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(nid) || nid.Length != 14 || !nid.All(char.IsDigit))
                {
                    Console.WriteLine("National ID must be exactly 14 digits.");
                    nid = null;
                }
            } while (nid == null);

            return nid;
        }


        public static int ReadInt(string prompt)
        {
            int num;
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out num))
                    return num;
                Console.WriteLine(" Invalid number. Try again.");
            }
        }

        public static decimal ReadDecimal(string prompt)
        {
            decimal num;
            while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine(), out num) && num >= 0)
                    return num;
                Console.WriteLine(" Invalid amount. Must be >= 0.");
            }
        }

        public static DateTime ReadDate(string prompt)
        {
            DateTime date;
            while (true)
            {
                Console.Write(prompt);
                if (DateTime.TryParse(Console.ReadLine(), out date) && date < DateTime.Today)
                    return date;

                Console.WriteLine(" Invalid date. Must be in the past (yyyy-mm-dd).");
            }
        }
    }

    // ===== ACCOUNT CLASSES =====
    public abstract class Account
    {
        private static int accountSeed = 1;
        public int AccountNumber { get; private set; }
        public decimal Balance { get; protected set; }
        public DateTime DateOpened { get; private set; }
        public List<string> Transactions { get; private set; }

        protected Account(decimal initialBalance)
        {
            AccountNumber = accountSeed++;
            Balance = initialBalance;
            DateOpened = DateTime.Now;
            Transactions = new List<string> { $"Account opened with balance: {initialBalance}" };
        }

        public virtual void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine(" Amount must be positive.");
                return;
            }
            Balance += amount;
            Transactions.Add($"Deposited: {amount}, New Balance: {Balance}");
        }

        public virtual void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine(" Amount must be positive.");
                return;
            }
            if (Balance >= amount)
            {
                Balance -= amount;
                Transactions.Add($"Withdrew: {amount}, New Balance: {Balance}");
            }
            else
                Console.WriteLine(" Insufficient funds.");
        }

        public abstract void EndOfMonthProcess();
    }

    public class SavingsAccount : Account
    {
        public decimal InterestRate { get; private set; }

        public SavingsAccount(decimal initialBalance, decimal interestRate) : base(initialBalance)
        {
            InterestRate = interestRate;
        }

        public override void EndOfMonthProcess()
        {
            var interest = Balance * InterestRate / 100;
            Balance += interest;
            Transactions.Add($"Interest added: {interest}, New Balance: {Balance}");
        }
    }

    public class CurrentAccount : Account
    {
        public decimal OverdraftLimit { get; private set; }

        public CurrentAccount(decimal initialBalance, decimal overdraftLimit) : base(initialBalance)
        {
            OverdraftLimit = overdraftLimit;
        }

        public override void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine(" Amount must be positive.");
                return;
            }
            if (Balance + OverdraftLimit >= amount)
            {
                Balance -= amount;
                Transactions.Add($"Withdrew: {amount}, New Balance: {Balance}");
            }
            else
                Console.WriteLine(" Overdraft limit exceeded.");
        }

        public override void EndOfMonthProcess()
        {
            // No interest for current accounts
        }
    }

    // ===== CUSTOMER =====
    public class Customer
    {
        private static int customerSeed = 1;
        public int CustomerId { get; private set; }
        public string FullName { get; set; }
        public string NationalId { get; private set; }
        public DateTime DateOfBirth { get; set; }
        public List<Account> Accounts { get; private set; }

        public Customer(string fullName, string nationalId, DateTime dob)
        {
            CustomerId = customerSeed++;
            FullName = fullName;
            NationalId = nationalId;
            DateOfBirth = dob;
            Accounts = new List<Account>();
        }

        public decimal TotalBalance() => Accounts.Sum(a => a.Balance);
    }

    // ===== BANK =====
    public class Bank
    {
        public string Name { get; private set; }
        public string BranchCode { get; private set; }
        public List<Customer> Customers { get; private set; }

        public Bank(string name, string branchCode)
        {
            Name = name;
            BranchCode = branchCode;
            Customers = new List<Customer>();
        }

        public Customer FindCustomerByName(string name) =>
            Customers.FirstOrDefault(c => c.FullName.Equals(name, StringComparison.OrdinalIgnoreCase));

        public Customer FindCustomerByNationalId(string nid) =>
            Customers.FirstOrDefault(c => c.NationalId == nid);

        public void AddCustomer()
        {
            string name = InputHelper.ReadName("Full name: ");
            string nid;
            do
            {
                nid = InputHelper.ReadNationalId("National ID: ");
                if (FindCustomerByNationalId(nid) != null)
                {
                    Console.WriteLine(" National ID already exists. Try again.");
                    nid = null;
                }
            } while (nid == null);

            DateTime dob = InputHelper.ReadDate("Date of birth (yyyy-mm-dd): ");
            Customers.Add(new Customer(name, nid, dob));
            Console.WriteLine(" Customer added successfully.");
        }

        public void UpdateCustomer()
        {
            string nid = InputHelper.ReadNationalId("Enter National ID: ");
            var customer = FindCustomerByNationalId(nid);
            if (customer == null) { Console.WriteLine(" Customer not found."); return; }

            customer.FullName = InputHelper.ReadName("New full name: ");
            customer.DateOfBirth = InputHelper.ReadDate("New date of birth (yyyy-mm-dd): ");
            Console.WriteLine(" Customer updated.");
        }

        public void RemoveCustomer()
        {
            string nid = InputHelper.ReadNationalId("Enter National ID: ");
            var customer = FindCustomerByNationalId(nid);
            if (customer == null) { Console.WriteLine(" Customer not found."); return; }
            if (customer.Accounts.Any(a => a.Balance != 0))
            {
                Console.WriteLine(" Cannot remove customer with non-zero account balance.");
                return;
            }
            Customers.Remove(customer);
            Console.WriteLine(" Customer removed.");
        }

        public void SearchCustomer()
        {
            Console.WriteLine("1. Search by name");
            Console.WriteLine("2. Search by National ID");
            int choice = InputHelper.ReadInt("Choose: ");
            Customer c = choice == 1
                ? FindCustomerByName(InputHelper.ReadName("Enter name: "))
                : FindCustomerByNationalId(InputHelper.ReadNationalId("Enter National ID: "));

            if (c != null)
                Console.WriteLine($" Found: {c.FullName}, ID: {c.CustomerId}, Accounts: {c.Accounts.Count}");
            else
                Console.WriteLine(" Customer not found.");
        }

        public void AddAccount()
        {
            string nid = InputHelper.ReadNationalId("Enter National ID: ");
            var customer = FindCustomerByNationalId(nid);
            if (customer == null)
            {
                Console.WriteLine(" Customer not found.");
                return;
            }

            Console.WriteLine("1. Savings Account");
            Console.WriteLine("2. Current Account");
            int choice = InputHelper.ReadInt("Choose type: ");

            decimal initial = InputHelper.ReadDecimal("Initial balance: ");

            Account newAcc = null;

            if (choice == 1)
            {
                decimal rate = InputHelper.ReadDecimal("Interest rate %: ");
                newAcc = new SavingsAccount(initial, rate);
                customer.Accounts.Add(newAcc);
            }
            else if (choice == 2)
            {
                decimal limit = InputHelper.ReadDecimal("Overdraft limit: ");
                newAcc = new CurrentAccount(initial, limit);
                customer.Accounts.Add(newAcc);
            }
            else
            {
                Console.WriteLine(" Invalid account type.");
                return;
            }

            Console.WriteLine($" Account created. Account Number: {newAcc.AccountNumber} for customer {customer.FullName} (Customer ID: {customer.CustomerId})");
        }

        public void Deposit()
        {
            var account = FindAccountByPrompt();
            if (account != null)
                account.Deposit(InputHelper.ReadDecimal("Amount to deposit: "));
        }

        public void Withdraw()
        {
            var account = FindAccountByPrompt();
            if (account != null)
                account.Withdraw(InputHelper.ReadDecimal("Amount to withdraw: "));
        }

        public void Transfer()
        {
            Console.WriteLine("From account:");
            var from = FindAccountByPrompt();
            Console.WriteLine("To account:");
            var to = FindAccountByPrompt();

            if (from != null && to != null)
            {
                decimal amount = InputHelper.ReadDecimal("Amount to transfer: ");
                if (amount > 0 && ((from is CurrentAccount ca && from.Balance + ca.OverdraftLimit >= amount) || (from.Balance >= amount)))
                {
                    from.Withdraw(amount);
                    to.Deposit(amount);
                    from.Transactions.Add($"Transferred {amount} to account {to.AccountNumber}");
                    to.Transactions.Add($"Received {amount} from account {from.AccountNumber}");
                    Console.WriteLine(" Transfer complete.");
                }
                else
                {
                    Console.WriteLine(" Transfer failed. Check balance or overdraft limit.");
                }
            }
        }

        public void CustomerReport()
        {
            string nid = InputHelper.ReadNationalId("Enter National ID: ");
            var customer = FindCustomerByNationalId(nid);
            if (customer == null) { Console.WriteLine(" Customer not found."); return; }

            Console.WriteLine($"Customer: {customer.FullName} (ID: {customer.CustomerId})");
            Console.WriteLine($"Total balance across accounts: {customer.TotalBalance()}");
            foreach (var acc in customer.Accounts)
            {
                Console.WriteLine($"  Account {acc.AccountNumber} - Balance: {acc.Balance}, Opened: {acc.DateOpened}");
            }
        }

        public void BankReport()
        {
            Console.WriteLine($"=== Bank: {Name} ({BranchCode}) ===");
            foreach (var customer in Customers)
            {
                Console.WriteLine($"Customer: {customer.FullName} - Total Balance: {customer.TotalBalance()}");
                foreach (var acc in customer.Accounts)
                {
                    Console.WriteLine($"  Account {acc.AccountNumber} - Balance: {acc.Balance}, Opened: {acc.DateOpened}");
                }
            }
        }

        public void TransactionHistory()
        {
            var acc = FindAccountByPrompt();
            if (acc != null)
            {
                Console.WriteLine($"Transaction history for Account {acc.AccountNumber}:");
                foreach (var t in acc.Transactions)
                    Console.WriteLine(" - " + t);
            }
        }

        private Account FindAccountByPrompt()
        {
            string nid = InputHelper.ReadNationalId("Enter customer's National ID: ");
            var customer = FindCustomerByNationalId(nid);
            if (customer == null) { Console.WriteLine(" Customer not found."); return null; }

            int accNum = InputHelper.ReadInt("Enter account number: ");
            var account = customer.Accounts.FirstOrDefault(a => a.AccountNumber == accNum);
            if (account == null) Console.WriteLine(" Account not found.");
            return account;
        }
    }

    // ===== MAIN PROGRAM =====
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Welcome to Bank System ===");

            Bank bank = new Bank("National Bank of Egypt", "Cairo-001");

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n--- MENU ---");
                Console.WriteLine("1. Add Customer");
                Console.WriteLine("2. Update Customer");
                Console.WriteLine("3. Remove Customer");
                Console.WriteLine("4. Search Customer");
                Console.WriteLine("5. Add Account");
                Console.WriteLine("6. Deposit");
                Console.WriteLine("7. Withdraw");
                Console.WriteLine("8. Transfer");
                Console.WriteLine("9. Customer Report");
                Console.WriteLine("10. Bank Report");
                Console.WriteLine("11. Transaction History");
                Console.WriteLine("0. Exit");

                int choice = InputHelper.ReadInt("Choose option: ");
                switch (choice)
                {
                    case 1: bank.AddCustomer(); break;
                    case 2: bank.UpdateCustomer(); break;
                    case 3: bank.RemoveCustomer(); break;
                    case 4: bank.SearchCustomer(); break;
                    case 5: bank.AddAccount(); break;
                    case 6: bank.Deposit(); break;
                    case 7: bank.Withdraw(); break;
                    case 8: bank.Transfer(); break;
                    case 9: bank.CustomerReport(); break;
                    case 10: bank.BankReport(); break;
                    case 11: bank.TransactionHistory(); break;
                    case 0: running = false; break;
                    default: Console.WriteLine(" Invalid choice."); break;
                }
            }
        }
    }
}
