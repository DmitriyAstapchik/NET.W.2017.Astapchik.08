namespace BankAccountSystem
{
    /// <summary>
    /// Calculates account bonus points for deposit/withdrawal
    /// </summary>
    public interface IBonusPointsCalculator
    {
        /// <summary>
        /// calculates deposit bonus points for <paramref name="account"/>
        /// </summary>
        /// <param name="account">bank account</param>
        /// <param name="depositAmount">deposit amount</param>
        /// <returns>bonus points</returns>
        float CalculateDepositPoints(BankAccount account, decimal depositAmount);

        /// <summary>
        /// calculates withdrawal bonus points for <paramref name="account"/>
        /// </summary>
        /// <param name="account">bank account</param>
        /// <param name="withdrawalAmount">withdrawal amount</param>
        /// <returns>bonus points</returns>
        float CalculateWithdrawalPoints(BankAccount account, decimal withdrawalAmount);
    }
}
