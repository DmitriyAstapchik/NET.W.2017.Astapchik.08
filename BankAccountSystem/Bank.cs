using System;
using System.Collections.Generic;
using System.IO;

namespace BankAccountSystem
{
    /// <summary>
    /// Represents a bank with functionality of making deposits/withdrawals, creating/closing accounts.
    /// </summary>
    public class Bank
    {
        /// <summary>
        /// Minimum amount of deposit.
        /// </summary>
        private const ushort MINIMUM_DEPOSIT = 50;

        /// <summary>
        /// Minimum amount of withdrawal.
        /// </summary>
        private const ushort MINIMUM_WITHDRAWAL = 10;

        /// <summary>
        /// Account dictionary with IBAN as a key.
        /// </summary>
        private Dictionary<string, BankAccount> accounts;

        /// <summary>
        /// A file path to accounts binary file.
        /// </summary>
        private string accountsFilePath;

        /// <summary>
        /// Creates a new bank instance with accounts info from <paramref name="filePath"/>.
        /// </summary>
        /// <param name="filePath">a path to a binary file with accounts info</param>
        public Bank(string filePath)
        {
            accountsFilePath = filePath;
            ReadFromFile();
        }

        /// <summary>
        /// Possible types of bank account with max balance values.
        /// </summary>
        public enum AccountType : uint { Standart = 1000, Gold = 5000, Platinum = 10000 };

        /// <summary>
        /// Get accessor to an account dictionary
        /// </summary>
        public Dictionary<string, BankAccount> Accounts { get { return accounts; } }

        /// <summary>
        /// Opens a new account for <paramref name="holder"/> with <paramref name="startBalance"/>.
        /// </summary>
        /// <param name="holder">person's full name</param>
        /// <param name="startBalance">first deposit amount</param>
        /// <returns>IBAN of a new account</returns>
        /// <exception cref="ArgumentException">Start balance is lesser than minimal.</exception>
        public string OpenNewAccount(string holder, decimal startBalance)
        {
            if (startBalance < MINIMUM_DEPOSIT)
            {
                throw new ArgumentException($"Cannot create a bank account with balance lesser than {MINIMUM_DEPOSIT}");
            }

            var iban = IBANGenerator.GenerateIBAN();
            var account = new BankAccount(iban, holder, 0, 0);
            account.Deposit(startBalance);
            accounts.Add(account.IBAN, account);
            WriteToFile();

            return account.IBAN;
        }

        /// <summary>
        /// Closes an account with specified <paramref name="IBAN"/>.
        /// </summary>
        /// <param name="IBAN">IBAN of an account ot close</param>
        /// <exception cref="ArgumentException">Could not remove an account with specified <paramref name="IBAN"/>.</exception>
        public void CloseAccount(string IBAN)
        {
            if (!accounts.Remove(IBAN))
            {
                throw new ArgumentException("Could not remove an account with given IBAN");
            }

            WriteToFile();
        }

        /// <summary>
        /// Makes a deposit of <paramref name="amount"/> to an account with specified <paramref name="IBAN"/>.
        /// </summary>
        /// <param name="IBAN">IBAN of an account to make deposit</param>
        /// <param name="amount">deposit amount</param>
        /// <returns>new account balance</returns>
        /// <exception cref="ArgumentException">deposit amount is lesser than minimal</exception>
        public decimal MakeDeposit(string IBAN, decimal amount)
        {
            if (amount < MINIMUM_DEPOSIT)
            {
                throw new ArgumentException("Minimum deposit amount is " + MINIMUM_DEPOSIT.ToString("C"));
            }

            var account = accounts[IBAN];
            account.Deposit(amount);
            WriteToFile();

            return account.Balance;
        }

        /// <summary>
        /// Makes a withdrawal of <paramref name="amount"/> from an account with specified <paramref name="IBAN"/>
        /// </summary>
        /// <param name="IBAN">IBAN of an account to make withdrawal</param>
        /// <param name="amount">withdrawal amount</param>
        /// <returns>new account balance</returns>
        /// <exception cref="ArgumentException">withdrawal amount is lesser than minimal</exception>
        public decimal MakeWithdrawal(string IBAN, decimal amount)
        {
            if (amount < MINIMUM_WITHDRAWAL)
            {
                throw new ArgumentException("Minimum withdrawal amount is " + MINIMUM_WITHDRAWAL.ToString("C"));
            }

            var account = accounts[IBAN];
            account.Withdraw(amount);
            WriteToFile();

            return account.Balance;
        }

        /// <summary>
        /// Reads a binary file with accounts info and adds it to dictionary
        /// </summary>
        private void ReadFromFile()
        {
            using (var stream = new FileStream(accountsFilePath, FileMode.OpenOrCreate))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var accounts = new Dictionary<string, BankAccount>();

                    while (reader.PeekChar() != -1)
                    {
                        var iban = reader.ReadString();
                        var owner = reader.ReadString();
                        var balance = reader.ReadDecimal();
                        var bonus = reader.ReadSingle();

                        accounts.Add(iban, new BankAccount(iban, owner, balance, bonus));
                    }

                    this.accounts = accounts;
                }
            }
        }

        /// <summary>
        /// Overwrites a binary account info file with a new account info
        /// </summary>
        private void WriteToFile()
        {
            using (var stream = new FileStream(accountsFilePath, FileMode.Truncate))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    foreach (var account in accounts)
                    {
                        writer.Write(account.Value.IBAN);
                        writer.Write(account.Value.owner);
                        writer.Write(account.Value.Balance);
                        writer.Write(account.Value.BonusPoints);
                    }
                }
            }
        }

        /// <summary>
        /// Identifies a type of specified <paramref name="account"/>.
        /// </summary>
        /// <param name="account">a bank account to specify its type</param>
        /// <returns>account type</returns>
        internal static AccountType GetAccountType(BankAccount account)
        {
            if (account.Balance <= (uint)AccountType.Standart)
            {
                return AccountType.Standart;
            }
            else if (account.Balance <= (uint)AccountType.Gold)
            {
                return AccountType.Gold;
            }
            else
            {
                return AccountType.Platinum;
            }
        }

        /// <summary>
        /// Calculation logic of account bonus points
        /// </summary>
        internal static class BonusPointsCalculator
        {
            private const ushort EFFECTIVE_AMOUNT = 10000;
            internal const ushort MAX_VALUE = 100;

            internal static float CalculateDepositPoints(decimal depositAmount, AccountType type)
            {
                return (float)depositAmount / EFFECTIVE_AMOUNT * (GetBalanceValue(type) + GetDepositValue(type));
            }

            internal static float CalculateWithdrawalPoints(decimal withdrawalAmount, AccountType type)
            {
                return (float)withdrawalAmount / EFFECTIVE_AMOUNT * GetBalanceValue(type);
            }

            private static byte GetDepositValue(AccountType type)
            {
                return (byte)Math.Log10((uint)type);
            }

            private static byte GetBalanceValue(AccountType type)
            {
                return (byte)((uint)type / 1000);
            }
        }

        /// <summary>
        /// Bank's IBAN generator
        /// </summary>
        private static class IBANGenerator
        {
            internal static string GenerateIBAN()
            {
                return Guid.NewGuid().ToString(); // some bank logic
            }
        }
    }
}
