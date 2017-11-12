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
        /// an amount of money that considers effective for a bank
        /// </summary>
        private const ushort EFFECTIVE_AMOUNT = 10000;

        /// <summary>
        /// maximum possible bonus points
        /// </summary>
        private const ushort MAX_POINTS = 100;

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
        /// 'value' of a balance
        /// </summary>
        protected byte balanceValue;

        /// <summary>
        /// 'value' of a deposit
        /// </summary>
        protected byte depositValue;
        #endregion

        #region constructors
        /// <summary>
        /// Creates a bank account instance with specified IBAN, owner, balance and bonus points.
        /// </summary>
        /// <param name="IBAN">International Bank Account Number</param>
        /// <param name="owner">owner</param>
        /// <param name="balance">balance</param>
        /// <param name="bonusPoints">bonus points</param>
        protected BankAccount(string IBAN, string owner, decimal balance, float bonusPoints)
        {
            if (string.IsNullOrWhiteSpace(IBAN))
            {
                throw new ArgumentException("String is not significant.", "IBAN");
            }

            if (string.IsNullOrWhiteSpace(owner))
            {
                throw new ArgumentException("String is not significant.", "owner");
            }

            if (bonusPoints < 0 || bonusPoints > MAX_POINTS)
            {
                throw new ArgumentOutOfRangeException("bonusPoints", $"Bonus points range is from 0 to {MAX_POINTS}.");
            }

            this.IBAN = IBAN;
            this.owner = owner;
            this.balance = balance;
            this.bonusPoints = bonusPoints;
            this.balanceValue = balanceValue;
            this.depositValue = depositValue;
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
            private set => bonusPoints = value < 0 ? 0 : value > MAX_POINTS ? MAX_POINTS : value;
        }
        #endregion

        #region methods
        /// <summary>
        /// Gets full account info string.
        /// </summary>
        /// <returns>account info as vertical list</returns>
        public override string ToString()
        {
            return string.Join(Environment.NewLine, $"IBAN: {IBAN}", $"Owner: {owner}", $"Balance: {Balance.ToString("C")}", $"Bonus points: {BonusPoints}");
        }

        /// <summary>
        /// Makes a deposit of an amount of money.
        /// </summary>
        /// <param name="money">an amount of money to deposit</param>
        internal protected void Deposit(decimal money)
        {
            balance += money;
            BonusPoints += ((IBonusPointsCalculator)this).CalculateDepositPoints(money);
        }

        /// <summary>
        /// Makes a withdrawal of an amount of money.
        /// </summary>
        /// <param name="money">an amount of money to withdraw</param>
        /// <exception cref="ArgumentException">account balance is lesser than <paramref name="money"/></exception>
        internal protected void Withdraw(decimal money)
        {
            if (balance < money)
            {
                throw new ArgumentException("Account balance is lesser than the requested withdrawal amount");
            }

            balance -= money;
            BonusPoints -= ((IBonusPointsCalculator)this).CalculateWithdrawalPoints(money);
        }

        /// <summary>
        /// calculates deposit bonus points
        /// </summary>
        /// <param name="depositAmount">deposit amount</param>
        /// <returns>bonus points</returns>
        float IBonusPointsCalculator.CalculateDepositPoints(decimal depositAmount)
        {
            return (float)depositAmount / EFFECTIVE_AMOUNT * (balanceValue + depositValue);
        }

        /// <summary>
        /// calculates withdrawal bonus points
        /// </summary>
        /// <param name="withdrawalAmount">withdrawal amount</param>
        /// <returns>bonus points</returns>
        float IBonusPointsCalculator.CalculateWithdrawalPoints(decimal withdrawalAmount)
        {
            return (float)withdrawalAmount / EFFECTIVE_AMOUNT * balanceValue;
        }
        #endregion
    }

    /// <summary>
    /// A bank account of standart type
    /// </summary>
    class StandartAccount : BankAccount
    {
        /// <summary>
        /// constructs a standart account with specified IBAN, owner, balance and bonus points
        /// </summary>
        /// <param name="iban">account IBAN</param>
        /// <param name="owner">account owner</param>
        /// <param name="balance">account balance</param>
        /// <param name="bonusPoints">account bonus points</param>
        internal StandartAccount(string iban, string owner, decimal balance, float bonusPoints) : base(iban, owner, balance, bonusPoints)
        {
            balanceValue = 1;
            depositValue = 3;
        }
    }

    /// <summary>
    /// A bank account of gold type
    /// </summary>
    class GoldAccount : BankAccount
    {
        /// <summary>
        /// constructs a gold account with specified IBAN, owner, balance and bonus points
        /// </summary>
        /// <param name="iban">account IBAN</param>
        /// <param name="owner">account owner</param>
        /// <param name="balance">account balance</param>
        /// <param name="bonusPoints">account bonus points</param>
        internal GoldAccount(string iban, string owner, decimal balance, float bonusPoints) : base(iban, owner, balance, bonusPoints)
        {
            balanceValue = 5;
            depositValue = 4;
        }
    }

    /// <summary>
    /// A bank account of platinum type
    /// </summary>
    class PlatinumAccount : BankAccount
    {
        /// <summary>
        /// constructs a platinum account with specified IBAN, owner, balance and bonus points
        /// </summary>
        /// <param name="iban">account IBAN</param>
        /// <param name="owner">account owner</param>
        /// <param name="balance">account balance</param>
        /// <param name="bonusPoints">account bonus points</param>
        internal PlatinumAccount(string iban, string owner, decimal balance, float bonusPoints) : base(iban, owner, balance, bonusPoints)
        {
            balanceValue = 10;
            depositValue = 5;
        }
    }
}
