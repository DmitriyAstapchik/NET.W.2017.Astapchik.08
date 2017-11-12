namespace BankAccountSystem
{
    /// <summary>
    /// Service funcionality to work with a bank account
    /// </summary>
    interface IBankAccountService : IIBANGenerator
    {
        /// <summary>
        /// Opens a new bank account
        /// </summary>
        /// <param name="holder">holder's full name</param>
        /// <param name="startBalance">start balance</param>
        /// <returns>IBAN of a new account</returns>
        string OpenNewAccount(string holder, decimal startBalance);

        /// <summary>
        /// Closes an account with specified IBAN
        /// </summary>
        /// <param name="IBAN">IBAN of an account to close</param>
        /// <returns>account balance</returns>
        decimal CloseAccount(string IBAN);

        /// <summary>
        /// Makes a deposit of money
        /// </summary>
        /// <param name="IBAN">IBAN of an account</param>
        /// <param name="amount">deposit amount</param>
        /// <returns>new account balance</returns>
        decimal MakeDeposit(string IBAN, decimal amount);

        /// <summary>
        /// Makes a withdrawal of money
        /// </summary>
        /// <param name="IBAN">IBAN of an account</param>
        /// <param name="amount">withdrawal amount</param>
        /// <returns>new account balance</returns>
        decimal MakeWithdrawal(string IBAN, decimal amount);
    }
}
