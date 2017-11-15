using System;

namespace BankAccountSystem
{
    /// <summary>
    /// Represents a bank account with IBAN, owner, balance, bonus points and account type
    /// </summary>
    public abstract class BankAccount
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
        internal const ushort EFFECTIVEAMOUNT = 10000;

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
            Balance = balance;
            BonusPoints = bonusPoints;
        }
        #endregion

        #region properties
        /// <summary>
        /// Gets an account balance.
        /// </summary>
        public decimal Balance { get => balance; internal set => balance = value; }

        /// <summary>
        /// Gets publicly and sets privately account bonus points using calculation.
        /// </summary>
        public float BonusPoints
        {
            get => (float)Math.Round(bonusPoints, 2);
            internal set => bonusPoints = value < 0 ? 0 : value > MAXPOINTS ? MAXPOINTS : value;
        }

        /// <summary>
        /// 'value' of a balance
        /// </summary>
        protected internal byte BalanceValue { get; protected set; }

        /// <summary>
        /// 'value' of a deposit
        /// </summary>
        protected internal byte DepositValue { get; protected set; }
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
        #endregion
    }
}
