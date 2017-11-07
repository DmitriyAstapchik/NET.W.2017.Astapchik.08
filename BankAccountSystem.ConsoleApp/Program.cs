using System;
using System.Linq;
using System.Globalization;

namespace BankAccountSystem.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = "Accounts";
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-us");

            var bank1 = new Bank(filePath);
            Console.WriteLine("- create a new bank instance with an empty file");

            var iban1 = bank1.OpenNewAccount("name1", 100);
            Console.WriteLine($"- open a new {bank1.Accounts[iban1].Type} account, IBAN: {iban1}");
            var balance1 = bank1.MakeDeposit(iban1, 1300);
            Console.WriteLine($"- make a deposit, new balance: {balance1}");
            balance1 = bank1.MakeWithdrawal(iban1, 1000);
            Console.WriteLine($"- make a withdrawal, new balance: {balance1}");
            Console.WriteLine($"- full account info:\n{bank1.Accounts[iban1]}");

            var iban2 = bank1.OpenNewAccount("name2", 5000);
            Console.WriteLine($"\n- open a new {bank1.Accounts[iban2].Type} account, IBAN: {iban2}");
            var balance2 = bank1.MakeDeposit(iban2, 11000);
            Console.WriteLine($"- make a deposit, new balance: {balance2}");
            balance2 = bank1.MakeWithdrawal(iban2, 9999);
            Console.WriteLine($"- make a withdrawal, new balance: {balance2}");
            Console.WriteLine($"- full account info:\n{bank1.Accounts[iban2]}");

            var bank2 = new Bank(filePath);
            Console.WriteLine("\n- create another bank instance with a non-empty file");

            Console.WriteLine("- print account info from file:");
            foreach (var account in bank2.Accounts)
            {
                Console.WriteLine($"\n{account.Value}");
            }

            while (bank2.Accounts.Count > 0)
            {
                bank2.CloseAccount(bank2.Accounts.Last().Key);
            }
            Console.WriteLine($"\n- close all accounts, accounts remaining: {bank2.Accounts.Count}");
            Console.WriteLine($"accounts file size: {new System.IO.FileInfo(filePath).Length} bytes");

            Console.Read();
        }
    }
}
