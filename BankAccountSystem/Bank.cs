using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace BankAccountSystem
{
    /// <summary>
    /// Represents a bank with functionality of making deposits/withdrawals, creating/closing accounts.
    /// </summary>
    public class Bank : IAccountService
    {
        #region fields
        /// <summary>
        /// Minimum amount of deposit.
        /// </summary>
        private const ushort MINDEPOSIT = 50;

        /// <summary>
        /// Minimum amount of withdrawal.
        /// </summary>
        private const ushort MINWITHDRAWAL = 10;

        /// <summary>
        /// bank accounts storage
        /// </summary>
        private IAccountStorage storage;

        /// <summary>
        /// Bank's IBAN generator
        /// </summary>
        private IIBANGenerator ibanGenerator;

        /// <summary>
        /// bonus points calculator
        /// </summary>
        private IBonusPointsCalculator calculator;
        #endregion

        #region constructors
        /// <summary>
        /// Creates a new bank instance with specified storage, IBAN generator and bonus points calculator
        /// </summary>
        /// <param name="storage">bank accounts storage</param>
        /// <param name="generator">IBAN generator</param>
        public Bank(IAccountStorage storage, IIBANGenerator ibanGenerator, IBonusPointsCalculator calculator)
        {
            this.storage = storage;
            this.ibanGenerator = ibanGenerator;
            this.calculator = calculator;
        }
        #endregion

        #region methods
        /// <summary>
        /// Opens a new account for <paramref name="holder"/> with <paramref name="startBalance"/>.
        /// </summary>
        /// <param name="holder">person's full name</param>
        /// <param name="startBalance">first deposit amount</param>
        /// <returns>IBAN of a new account</returns>
        /// <exception cref="ArgumentException">Start balance is lesser than minimal.</exception>
        public string OpenNewAccount(string holder, decimal startBalance)
        {
            if (string.IsNullOrWhiteSpace(holder))
            {
                throw new ArgumentException("No significant characters are given.", "holder");
            }

            if (startBalance < MINDEPOSIT)
            {
                throw new ArgumentException($"Cannot create a bank account with balance lesser than {MINDEPOSIT}");
            }

            BankAccount account;
            if (startBalance <= 1000)
            {
                account = new StandardAccount(ibanGenerator.GenerateIBAN(), holder, startBalance, bonusPoints: 0);
            }
            else if (startBalance <= 10000)
            {
                account = new GoldAccount(ibanGenerator.GenerateIBAN(), holder, startBalance, bonusPoints: 5);
            }
            else
            {
                account = new PlatinumAccount(ibanGenerator.GenerateIBAN(), holder, startBalance, bonusPoints: 10);
            }

            storage.AddAccount(account);

            return account.IBAN;
        }

        /// <summary>
        /// Removes an account with specified <paramref name="iban"/> from accounts file
        /// </summary>
        /// <param name="iban">IBAN of an account to close</param>
        /// <returns>account balance</returns>
        public decimal CloseAccount(string iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
            {
                throw new ArgumentException("No IBAN is given.", "IBAN");
            }

            return storage.RemoveAccount(iban);
        }

        /// <summary>
        /// Makes a deposit of <paramref name="amount"/> to an account with specified <paramref name="iban"/>.
        /// </summary>
        /// <param name="iban">IBAN of an account to make deposit</param>
        /// <param name="amount">deposit amount</param>
        /// <returns>new account balance</returns>
        /// <exception cref="ArgumentException">deposit amount is lesser than minimal</exception>
        public decimal MakeDeposit(string iban, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(iban))
            {
                throw new ArgumentException("No IBAN is given.", "IBAN");
            }

            if (amount < MINDEPOSIT)
            {
                throw new ArgumentException("Minimum deposit amount is " + MINDEPOSIT.ToString("C"));
            }

            var account = storage.GetAccount(iban);
            account.Balance += amount;
            account.BonusPoints += calculator.CalculateDepositPoints(account, amount);
            storage.SaveAccount(account);

            return account.Balance;
        }

        /// <summary>
        /// Makes a withdrawal of <paramref name="amount"/> from an account with specified <paramref name="iban"/>
        /// </summary>
        /// <param name="iban">IBAN of an account to make withdrawal</param>
        /// <param name="amount">withdrawal amount</param>
        /// <returns>new account balance</returns>
        /// <exception cref="ArgumentException">withdrawal amount is lesser than minimal</exception>
        public decimal MakeWithdrawal(string iban, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(iban))
            {
                throw new ArgumentException("No IBAN is given.", "IBAN");
            }

            if (amount < MINWITHDRAWAL)
            {
                throw new ArgumentException("Minimum withdrawal amount is " + MINWITHDRAWAL.ToString("C"));
            }

            var account = storage.GetAccount(iban);
            account.Balance -= amount;
            account.BonusPoints -= calculator.CalculateWithdrawalPoints(account, amount);
            storage.SaveAccount(account); 

            return account.Balance;
        }
        #endregion
    }
}
