using System;
using System.IO;
using System.Reflection;

namespace BankAccountSystem
{
    /// <summary>
    /// Represents a bank with functionality of making deposits/withdrawals, creating/closing accounts.
    /// </summary>
    public class Bank : IBankAccountService, IAccountFileService
    {
        #region fields
        /// <summary>
        /// Minimum amount of deposit.
        /// </summary>
        private const ushort MIN_DEPOSIT = 50;

        /// <summary>
        /// Minimum amount of withdrawal.
        /// </summary>
        private const ushort MIN_WITHDRAWAL = 10;

        /// <summary>
        /// A file path to accounts binary file.
        /// </summary>
        private string accountsFilePath;

        /// <summary>
        /// Bank's IBAN generator
        /// </summary>
        private IIBANGenerator IBANGenerator;
        #endregion

        #region constructors
        /// <summary>
        /// Creates a new bank instance with accounts info from <paramref name="filePath"/>.
        /// </summary>
        /// <param name="filePath">a path to a binary file with accounts info</param>
        public Bank(string filePath, IIBANGenerator generator)
        {
            accountsFilePath = filePath;
            IBANGenerator = generator;
        }
        #endregion

        #region methods
        /// <summary>
        /// Opens a new account for <paramref name="holder"/> with <paramref name="startBalance"/>.
        /// </summary>
        /// <param name="holder">person's full name</param>
        /// <param name="startBalance">first deposit amount</param>
        /// <returns>IBAN of a new account</returns>
        /// <exception cref="ArgumentException">Start balance is lesser than minimal.</exception>
        public string OpenNewAccount(string holder, decimal startBalance)
        {
            if (string.IsNullOrWhiteSpace(holder))
            {
                throw new ArgumentException("No significant characters are given.", "holder");
            }

            if (startBalance < MIN_DEPOSIT)
            {
                throw new ArgumentException($"Cannot create a bank account with balance lesser than {MIN_DEPOSIT}");
            }

            BankAccount account;
            if (startBalance <= 1000)
            {
                account = new StandartAccount(IBANGenerator.GenerateIBAN(), holder, startBalance, bonusPoints: 0);
            }
            else if (startBalance <= 10000)
            {
                account = new GoldAccount(IBANGenerator.GenerateIBAN(), holder, startBalance, bonusPoints: 5);
            }
            else
            {
                account = new PlatinumAccount(IBANGenerator.GenerateIBAN(), holder, startBalance, bonusPoints: 10);
            }
            ((IAccountFileService)this).AddAccount(account);

            return account.IBAN;
        }

        /// <summary>
        /// Removes an account with specified <paramref name="IBAN"/> from accounts file
        /// </summary>
        /// <param name="IBAN">IBAN of an account to close</param>
        /// <returns>account balance</returns>
        public decimal CloseAccount(string IBAN)
        {
            if (string.IsNullOrWhiteSpace(IBAN))
            {
                throw new ArgumentException("No IBAN is given.", "IBAN");
            }

            return ((IAccountFileService)this).RemoveAccount(IBAN);
        }

        /// <summary>
        /// Makes a deposit of <paramref name="amount"/> to an account with specified <paramref name="IBAN"/>.
        /// </summary>
        /// <param name="IBAN">IBAN of an account to make deposit</param>
        /// <param name="amount">deposit amount</param>
        /// <returns>new account balance</returns>
        /// <exception cref="ArgumentException">deposit amount is lesser than minimal</exception>
        public decimal MakeDeposit(string IBAN, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(IBAN))
            {
                throw new ArgumentException("No IBAN is given.", "IBAN");
            }

            if (amount < MIN_DEPOSIT)
            {
                throw new ArgumentException("Minimum deposit amount is " + MIN_DEPOSIT.ToString("C"));
            }

            var account = ReadFromBinary(IBAN);
            account.Deposit(amount);
            WriteToBinary(account);

            return account.Balance;
        }

        /// <summary>
        /// Makes a withdrawal of <paramref name="amount"/> from an account with specified <paramref name="IBAN"/>
        /// </summary>
        /// <param name="IBAN">IBAN of an account to make withdrawal</param>
        /// <param name="amount">withdrawal amount</param>
        /// <returns>new account balance</returns>
        /// <exception cref="ArgumentException">withdrawal amount is lesser than minimal</exception>
        public decimal MakeWithdrawal(string IBAN, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(IBAN))
            {
                throw new ArgumentException("No IBAN is given.", "IBAN");
            }

            if (amount < MIN_WITHDRAWAL)
            {
                throw new ArgumentException("Minimum withdrawal amount is " + MIN_WITHDRAWAL.ToString("C"));
            }

            var account = ReadFromBinary(IBAN);
            account.Withdraw(amount);
            WriteToBinary(account);

            return account.Balance;
        }

        /// <summary>
        /// Generates an IBAN
        /// </summary>
        /// <returns>IBAN</returns>
        string IIBANGenerator.GenerateIBAN()
        {
            return IBANGenerator.GenerateIBAN();
        }

        /// <summary>
        /// Adds an account to a file
        /// </summary>
        /// <param name="account">account to add</param>
        void IAccountFileService.AddAccount(BankAccount account)
        {
            AddToBinary(account);
        }

        /// <summary>
        /// Removes an account from a file
        /// </summary>
        /// <param name="IBAN">IBAN of an account to remove</param>
        /// <param name="balance">account balance</param>
        /// <returns>account balance</returns>
        decimal IAccountFileService.RemoveAccount(string IBAN)
        {
            return RemoveFromBinary(IBAN);
        }

        /// <summary>
        /// Gets an account from a file
        /// </summary>
        /// <param name="IBAN">IBAN of an account</param>
        /// <returns>bank account instance</returns>
        BankAccount IAccountFileService.ReadAccount(string IBAN)
        {
            return ReadFromBinary(IBAN);
        }

        /// <summary>
        /// Writes an account to a file
        /// </summary>
        /// <param name="account">account to write</param>
        void IAccountFileService.WriteAccount(BankAccount account)
        {
            WriteToBinary(account);
        }

        /// <summary>
        /// adds an account to a binary file
        /// </summary>
        /// <param name="account">account to add</param>
        private void AddToBinary(BankAccount account)
        {
            using (var stream = new FileStream(accountsFilePath, FileMode.Append))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(account.IBAN);
                    writer.Write(account.owner);
                    writer.Write(account.Balance);
                    writer.Write(account.BonusPoints);
                    writer.Write(account.GetType().FullName);
                }
            }
        }

        /// <summary>
        /// removes an account from binary file
        /// </summary>
        /// <param name="IBAN">IBAN of an account to remove</param>
        /// <returns>account balance</returns>
        private decimal RemoveFromBinary(string IBAN)
        {
            using (var stream = new FileStream(accountsFilePath, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    while (reader.ReadString() != IBAN)
                    {
                        reader.ReadString();
                        reader.ReadDecimal();
                        reader.ReadSingle();
                        reader.ReadString();
                    }
                    var owner = reader.ReadString();
                    var balance = reader.ReadDecimal();
                    reader.ReadSingle();
                    var type = reader.ReadString();
                    var offset = (IBAN.Length + 1) + (owner.Length + 1) + sizeof(decimal) + sizeof(float) + (type.Length + 1);
                    //stream.Seek(-offset, SeekOrigin.Current);
                    byte[] array = new byte[stream.Length - stream.Position];
                    stream.Read(array, 0, array.Length);
                    //reader.ReadSingle();
                    //var type = reader.ReadString();
                    //reader.BaseStream.Position -= (IBAN.Length + 1) + (owner.Length + 1) + sizeof(decimal) + sizeof(float) + (type.Length + 1);
                    //reader.BaseStream.SetLength(reader.BaseStream.Position);
                    using (var writer = new BinaryWriter(stream))
                    {
                        writer.Seek(-(array.Length + offset), SeekOrigin.Current);
                        writer.Write(array);
                        stream.SetLength(stream.Position);
                    }

                    return balance;
                }
            }
        }

        /// <summary>
        /// Gets an account with specified IBAN from a binary file
        /// </summary>
        /// <param name="IBAN">IBAN to find</param>
        /// <returns>bank account instance</returns>
        private BankAccount ReadFromBinary(string IBAN)
        {
            BankAccount account;

            using (var stream = new FileStream(accountsFilePath, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    while (reader.ReadString() != IBAN)
                    {
                        reader.ReadString();
                        reader.ReadDecimal();
                        reader.ReadSingle();
                        reader.ReadString();
                    }

                    var owner = reader.ReadString();
                    var balance = reader.ReadDecimal();
                    var bonus = reader.ReadSingle();
                    var type = reader.ReadString();
                    account = (BankAccount)Type.GetType(type).GetConstructors(BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.NonPublic)[0].Invoke(new object[] { IBAN, owner, balance, bonus });
                }
            }

            return account;
        }

        /// <summary>
        /// Writes account info to a binary file
        /// </summary>
        /// <param name="account">account to save</param>
        private void WriteToBinary(BankAccount account)
        {
            using (var stream = new FileStream(accountsFilePath, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    while (reader.ReadString() != account.IBAN)
                    {
                        reader.ReadString();
                        reader.ReadDecimal();
                        reader.ReadSingle();
                        reader.ReadString();
                    }

                    using (var writer = new BinaryWriter(stream))
                    {
                        writer.Write(account.owner);
                        writer.Write(account.Balance);
                        writer.Write(account.BonusPoints);
                        writer.Write(account.GetType().FullName);
                    }
                }
            }
        }
        #endregion
    }
}
