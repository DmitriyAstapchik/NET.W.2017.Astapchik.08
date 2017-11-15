namespace BankAccountSystem
{
    /// <summary>
    /// Allows to work with a storage of bank accounts
    /// </summary>
    public interface IAccountStorage
    {
        /// <summary>
        /// Adds an account to a storage
        /// </summary>
        /// <param name="account">an account to add</param>
        void AddAccount(BankAccount account);

        /// <summary>
        /// Removes an account from a storage
        /// </summary>
        /// <param name="iban">IBAN of an account</param>
        /// <returns>account balance</returns>
        decimal RemoveAccount(string iban);

        /// <summary>
        /// Gets an account from a storage
        /// </summary>
        /// <param name="iban">IBAN of an account</param>
        /// <returns>bank account instance</returns>
        BankAccount GetAccount(string iban);

        /// <summary>
        /// Rewrites an account in a storage
        /// </summary>
        /// <param name="account">account to rewrite</param>
        void SaveAccount(BankAccount account);
    }
}
