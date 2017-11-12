namespace BankAccountSystem
{
    /// <summary>
    /// A bank account of standard type
    /// </summary>
    internal class StandardAccount : BankAccount
    {
        /// <summary>
        /// constructs a standard account with specified IBAN, owner, balance and bonus points
        /// </summary>
        /// <param name="iban">account IBAN</param>
        /// <param name="owner">account owner</param>
        /// <param name="balance">account balance</param>
        /// <param name="bonusPoints">account bonus points</param>
        internal StandardAccount(string iban, string owner, decimal balance, float bonusPoints) : base(iban, owner, balance, bonusPoints)
        {
            this.BalanceValue = 1;
            this.DepositValue = 3;
        }
    }
}
