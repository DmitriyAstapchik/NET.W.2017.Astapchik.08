namespace BankAccountSystem
{
    /// <summary>
    /// Functionality to generate an IBAN
    /// </summary>
    public interface IIBANGenerator
    {
        /// <summary>
        /// Generates an IBAN
        /// </summary>
        /// <returns>IBAN</returns>
        string GenerateIBAN();
    }
}
