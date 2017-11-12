namespace BankAccountSystem
{
    /// <summary>
    /// Allows to work with bank accounts from a file directly
    /// </summary>
    interface IAccountFileService
    {
        /// <summary>
        /// Adds an account to a file
        /// </summary>
        /// <param name="account">an account to add</param>
        void AddAccount(BankAccount account);

        /// <summary>
        /// Removes an account from a file
        /// </summary>
        /// <param name="IBAN">IBAN of an account</param>
        /// <returns>account balance</returns>
        decimal RemoveAccount(string IBAN);

        /// <summary>
        /// Gets an account from a file
        /// </summary>
        /// <param name="IBAN">IBAN of an account</param>
        /// <returns>bank account instance</returns>
        BankAccount ReadAccount(string IBAN);

        /// <summary>
        /// Writes an account to a file
        /// </summary>
        /// <param name="account">account to write</param>
        void WriteAccount(BankAccount account);
    }
}
