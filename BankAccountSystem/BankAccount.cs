using System;

namespace BankAccountSystem
{
    /// <summary>
    /// Represents a bank account with IBAN, owner, balance, bonus points and account type. Allows to make deposits and withdrawals.
    /// </summary>
    public abstract class BankAccount : IBonusPointsCalculator
    {
        #region fields
        /// <summary>
        /// International Bank Account Number of an account.
        /// </summary>
        public readonly string IBAN;

        /// <summary>
        /// Owner of an account.
        /// </summary>
        public readonly string Owner;

        /// <summary>
        /// an amount of money that considers effective for a bank
        /// </summary>
        private const ushort EFFECTIVEAMOUNT = 10000;

        /// <summary>
        /// maximum possible bonus points
        /// </summary>
        private const ushort MAXPOINTS = 100;

        /// <summary>
        /// Balance of an account.
        /// </summary>
        private decimal balance;

        /// <summary>
        /// Bonus points of an account.
        /// </summary>
        private float bonusPoints;
        #endregion

        #region constructors
        /// <summary>
        /// Creates a bank account instance with specified IBAN, owner, balance and bonus points.
        /// </summary>
        /// <param name="iban">International Bank Account Number</param>
        /// <param name="owner">owner</param>
        /// <param name="balance">balance</param>
        /// <param name="bonusPoints">bonus points</param>
        protected BankAccount(string iban, string owner, decimal balance, float bonusPoints)
        {
            if (string.IsNullOrWhiteSpace(iban))
            {
                throw new ArgumentException("String is not significant.", "IBAN");
            }

            if (string.IsNullOrWhiteSpace(owner))
            {
                throw new ArgumentException("String is not significant.", "owner");
            }

            if (bonusPoints < 0 || bonusPoints > MAXPOINTS)
            {
                throw new ArgumentOutOfRangeException("bonusPoints", $"Bonus points range is from 0 to {MAXPOINTS}.");
            }

            this.IBAN = iban;
            this.Owner = owner;
            this.balance = balance;
            this.bonusPoints = bonusPoints;
        }
        #endregion

        #region properties
        /// <summary>
        /// Gets an account balance.
        /// </summary>
        public decimal Balance => balance;

        /// <summary>
        /// Gets publicly and sets privately account bonus points using calculation.
        /// </summary>
        public float BonusPoints
        {
            get => (float)Math.Round(bonusPoints, 2);
            private set => bonusPoints = value < 0 ? 0 : value > MAXPOINTS ? MAXPOINTS : value;
        }

        /// <summary>
        /// 'value' of a balance
        /// </summary>
        protected byte BalanceValue { get => BalanceValue; set => BalanceValue = value; }

        /// <summary>
        /// 'value' of a deposit
        /// </summary>
        protected byte DepositValue { get => DepositValue; set => DepositValue = value; }
        #endregion

        #region methods
        /// <summary>
        /// Gets full account info string.
        /// </summary>
        /// <returns>account info as vertical list</returns>
        public override string ToString()
        {
            return string.Join(Environment.NewLine, $"IBAN: {IBAN}", $"Owner: {Owner}", $"Balance: {Balance.ToString("C")}", $"Bonus points: {BonusPoints}");
        }

        /// <summary>
        /// calculates deposit bonus points
        /// </summary>
        /// <param name="depositAmount">deposit amount</param>
        /// <returns>bonus points</returns>
        float IBonusPointsCalculator.CalculateDepositPoints(decimal depositAmount)
        {
            return (float)depositAmount / EFFECTIVEAMOUNT * (BalanceValue + DepositValue);
        }

        /// <summary>
        /// calculates withdrawal bonus points
        /// </summary>
        /// <param name="withdrawalAmount">withdrawal amount</param>
        /// <returns>bonus points</returns>
        float IBonusPointsCalculator.CalculateWithdrawalPoints(decimal withdrawalAmount)
        {
            return (float)withdrawalAmount / EFFECTIVEAMOUNT * BalanceValue;
        }

        /// <summary>
        /// Makes a deposit of an amount of money.
        /// </summary>
        /// <param name="money">an amount of money to deposit</param>
        protected internal void Deposit(decimal money)
        {
            balance += money;
            BonusPoints += ((IBonusPointsCalculator)this).CalculateDepositPoints(money);
        }

        /// <summary>
        /// Makes a withdrawal of an amount of money.
        /// </summary>
        /// <param name="money">an amount of money to withdraw</param>
        /// <exception cref="ArgumentException">account balance is lesser than <paramref name="money"/></exception>
        protected internal void Withdraw(decimal money)
        {
            if (balance < money)
            {
                throw new ArgumentException("Account balance is lesser than the requested withdrawal amount");
            }

            balance -= money;
            BonusPoints -= ((IBonusPointsCalculator)this).CalculateWithdrawalPoints(money);
        }
        #endregion
    }
}
