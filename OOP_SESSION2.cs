
using System;

namespace BankApplication
{
    class BankAccount
    {
        public const string BankCode = "BNK001";
        public readonly DateTime CreatedDate;
        private int _accountNumber;
        private string _fullName;
        private string _nationalID;
        private string _phoneNumber;
        private string _address;
        private decimal _balance;
        private static int _accountCounter = 1000;

        public string FullName
        {
            get => _fullName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("FullName must not be empty.");
                _fullName = value;
            }
        }

        public string NationalID
        {
            get => _nationalID;
            set
            {
                if (!IsValidNationalID(value))
                    throw new ArgumentException("NationalID must be exactly 14 digits.");
                _nationalID = value;
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (!IsValidPhoneNumber(value))
                    throw new ArgumentException("PhoneNumber must start with '01' and be 11 digits.");
                _phoneNumber = value;
            }
        }

        public string Address
        {
            get => _address;
            set => _address = value;
        }

        public decimal Balance
        {
            get => _balance;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Balance must be >= 0.");
                _balance = value;
            }
        }

        public BankAccount()
        {
            _accountNumber = ++_accountCounter;
            _fullName = "Unknown";
            _nationalID = "00000000000000";
            _phoneNumber = "01000000000";
            _address = "Not Provided";
            _balance = 0;
            CreatedDate = DateTime.Now;
        }

        public BankAccount(string fullName, string nationalID, string phoneNumber, string address, decimal balance)
        {
            _accountNumber = ++_accountCounter;
            FullName = fullName;
            NationalID = nationalID;
            PhoneNumber = phoneNumber;
            Address = address;
            Balance = balance;
            CreatedDate = DateTime.Now;
        }

        public BankAccount(string fullName, string nationalID, string phoneNumber, string address)
            : this(fullName, nationalID, phoneNumber, address, 0)
        {
        }

        public void ShowAccountDetails()
        {
            Console.WriteLine("=== Account Details ===");
            Console.WriteLine($"Bank Code: {BankCode}");
            Console.WriteLine($"Account Number: {_accountNumber}");
            Console.WriteLine($"Full Name: {FullName}");
            Console.WriteLine($"National ID: {NationalID}");
            Console.WriteLine($"Phone Number: {PhoneNumber}");
            Console.WriteLine($"Address: {Address}");
            Console.WriteLine($"Balance: {Balance:C}");
            Console.WriteLine($"Created Date: {CreatedDate}");
            Console.WriteLine("======================\n");
        }

        public bool IsValidNationalID(string nationalID)
        {
            return !string.IsNullOrWhiteSpace(nationalID) && nationalID.Length == 14 && long.TryParse(nationalID, out _);
        }

        public bool IsValidPhoneNumber(string phoneNumber)
        {
            return !string.IsNullOrWhiteSpace(phoneNumber) && phoneNumber.StartsWith("01") && phoneNumber.Length == 11 && long.TryParse(phoneNumber, out _);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            BankAccount account1 = new BankAccount();
            BankAccount account2 = new BankAccount("Nayira Abdelwahab", "12345678901234", "01234567890", "Cairo", 5000);
            account1.ShowAccountDetails();
            account2.ShowAccountDetails();
            BankAccount account3 = new BankAccount("Ali Ahmed", "98765432101234", "01122334455", "Alexandria");
            account3.ShowAccountDetails();
            Console.ReadLine();
        }
    }
}
