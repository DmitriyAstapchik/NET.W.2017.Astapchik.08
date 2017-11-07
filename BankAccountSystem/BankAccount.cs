using System;

namespace BankAccountSystem
{
    /// <summary>
    /// Represents a bank account with IBAN, owner, balance, bonus points and account type. Allows to make deposits and withdrawals.
    /// </summary>
    public class BankAccount
    {
        /// <summary>
        /// International Bank Account Number of an account.
        /// </summary>
        public readonly string IBAN;

        /// <summary>
        /// Owner of an account.
        /// </summary>
        public readonly string owner;

        /// <summary>
        /// Balance of an account.
        /// </summary>
        private decimal balance;

        /// <summary>
        /// Bonus points of an account.
        /// </summary>
        private float bonusPoints;

        /// <summary>
        /// Creates a bank account instance with specified IBAN, owner, balance and bonus points.
        /// </summary>
        /// <param name="IBAN">International Bank Account Number</param>
        /// <param name="owner">owner</param>
        /// <param name="balance">balance</param>
        /// <param name="bonusPoints">bonus points</param>
        internal BankAccount(string IBAN, string owner, decimal balance, float bonusPoints)
        {
            this.IBAN = IBAN;
            this.owner = owner;
            this.balance = balance;
            this.bonusPoints = bonusPoints;
        }

        /// <summary>
        /// Gets an account balance.
        /// </summary>
        public decimal Balance => balance;

        /// <summary>
        /// Gets an account type.
        /// </summary>
        public Bank.AccountType Type => Bank.GetAccountType(this);

        /// <summary>
        /// Gets publicly and sets privately account bonus points using calculation.
        /// </summary>
        public float BonusPoints
        {
            get => (float)Math.Round(bonusPoints, 2);
            private set => bonusPoints = value < 0 ? 0 : value > Bank.BonusPointsCalculator.MAX_VALUE ? Bank.BonusPointsCalculator.MAX_VALUE : value;
        }

        /// <summary>
        /// Gets full account info string.
        /// </summary>
        /// <returns>account info as vertical list</returns>
        public override string ToString()
        {
            return string.Join(Environment.NewLine, $"IBAN: {IBAN}", $"Owner: {owner}", $"Balance: {Balance.ToString("C")}", $"Bonus points: {BonusPoints}", $"Account type: {Type}");
        }

        /// <summary>
        /// Makes a deposit of an amount of money.
        /// </summary>
        /// <param name="money">an amount of money to deposit</param>
        internal void Deposit(decimal money)
        {
            balance += money;
            BonusPoints += Bank.BonusPointsCalculator.CalculateDepositPoints(money, Type);
        }

        /// <summary>
        /// Makes a withdrawal of an amount of money.
        /// </summary>
        /// <param name="money">an amount of money to withdraw</param>
        /// <exception cref="ArgumentException">account balance is lesser than <paramref name="money"/></exception>
        internal void Withdraw(decimal money)
        {
            if (balance < money)
            {
                throw new ArgumentException("Account balance is lesser than the requested withdrawal amount");
            }

            BonusPoints -= Bank.BonusPointsCalculator.CalculateWithdrawalPoints(money, Type);
            balance -= money;
        }
    }
}
