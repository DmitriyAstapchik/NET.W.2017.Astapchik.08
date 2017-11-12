namespace BankAccountSystem
{
    /// <summary>
    /// A bank account of platinum type
    /// </summary>
    internal class PlatinumAccount : BankAccount
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
            this.BalanceValue = 10;
            this.DepositValue = 5;
        }
    }
}
