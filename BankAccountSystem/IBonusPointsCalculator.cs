namespace BankAccountSystem
{
    /// <summary>
    /// Calculates bonus points for deposit/withdrawal
    /// </summary>
    interface IBonusPointsCalculator
    {
        /// <summary>
        /// calculates deposit bonus points
        /// </summary>
        /// <param name="depositAmount">deposit amount</param>
        /// <returns>bonus points</returns>
        float CalculateDepositPoints(decimal depositAmount);

        /// <summary>
        /// calculates withdrawal bonus points
        /// </summary>
        /// <param name="withdrawalAmount">withdrawal amount</param>
        /// <returns>bonus points</returns>
        float CalculateWithdrawalPoints(decimal withdrawalAmount);
    }
}
