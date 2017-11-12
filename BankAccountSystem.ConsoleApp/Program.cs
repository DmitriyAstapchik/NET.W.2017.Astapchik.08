using System;
using System.Globalization;

namespace BankAccountSystem.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var filePath = "Accounts";
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-us");

            var bank1 = new Bank(filePath, new Generator());
            Console.WriteLine("- create a new bank instance");

            var iban1 = bank1.OpenNewAccount("name1", 100);
            Console.WriteLine($"- open a new account, IBAN: {iban1}");
            var balance1 = bank1.MakeDeposit(iban1, 1300);
            Console.WriteLine($"- make a deposit, new balance: {balance1}");
            balance1 = bank1.MakeWithdrawal(iban1, 1000);
            Console.WriteLine($"- make a withdrawal, new balance: {balance1}");

            var iban2 = bank1.OpenNewAccount("name2", 5000);
            Console.WriteLine($"- open another account, IBAN: {iban2}");
            var balance2 = bank1.MakeDeposit(iban2, 3000);
            Console.WriteLine($"- make a deposit, new balance: {balance2}");
            balance2 = bank1.MakeWithdrawal(iban2, 1000);
            Console.WriteLine($"- make a withdrawal, new balance: {balance2}");

            var bank2 = new Bank(filePath, new Generator());
            Console.WriteLine("\n- create another bank instance from same file");

            bank2.CloseAccount(iban1);
            bank2.CloseAccount(iban2);
            Console.WriteLine($"\n- close all 2 accounts, accounts file size: {new System.IO.FileInfo(filePath).Length} bytes");

            Console.Read();
        }

        private class Generator : IIBANGenerator
        {
            public string GenerateIBAN()
            {
                return Guid.NewGuid().ToString();
            }
        }
    }
}
