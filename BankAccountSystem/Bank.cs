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
        private const ushort MINDEPOSIT = 50;

        /// <summary>
        /// Minimum amount of withdrawal.
        /// </summary>
        private const ushort MINWITHDRAWAL = 10;

        /// <summary>
        /// A file path to accounts binary file.
        /// </summary>
        private string accountsFilePath;

        /// <summary>
        /// Bank's IBAN generator
        /// </summary>
        private IIBANGenerator ibanGenerator;
        #endregion

        #region constructors
        /// <summary>
        /// Creates a new bank instance with accounts info from <paramref name="filePath"/>.
        /// </summary>
        /// <param name="filePath">a path to a binary file with accounts info</param>
        public Bank(string filePath, IIBANGenerator generator)
        {
            accountsFilePath = filePath;
            ibanGenerator = generator;
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

            if (startBalance < MINDEPOSIT)
            {
                throw new ArgumentException($"Cannot create a bank account with balance lesser than {MINDEPOSIT}");
            }

            BankAccount account;
            if (startBalance <= 1000)
            {
                account = new StandardAccount(ibanGenerator.GenerateIBAN(), holder, startBalance, bonusPoints: 0);
            }
            else if (startBalance <= 10000)
            {
                account = new GoldAccount(ibanGenerator.GenerateIBAN(), holder, startBalance, bonusPoints: 5);
            }
            else
            {
                account = new PlatinumAccount(ibanGenerator.GenerateIBAN(), holder, startBalance, bonusPoints: 10);
            }

            ((IAccountFileService)this).AddAccount(account);

            return account.IBAN;
        }

        /// <summary>
        /// Removes an account with specified <paramref name="iban"/> from accounts file
        /// </summary>
        /// <param name="iban">IBAN of an account to close</param>
        /// <returns>account balance</returns>
        public decimal CloseAccount(string iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
            {
                throw new ArgumentException("No IBAN is given.", "IBAN");
            }

            return ((IAccountFileService)this).RemoveAccount(iban);
        }

        /// <summary>
        /// Makes a deposit of <paramref name="amount"/> to an account with specified <paramref name="iban"/>.
        /// </summary>
        /// <param name="iban">IBAN of an account to make deposit</param>
        /// <param name="amount">deposit amount</param>
        /// <returns>new account balance</returns>
        /// <exception cref="ArgumentException">deposit amount is lesser than minimal</exception>
        public decimal MakeDeposit(string iban, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(iban))
            {
                throw new ArgumentException("No IBAN is given.", "IBAN");
            }

            if (amount < MINDEPOSIT)
            {
                throw new ArgumentException("Minimum deposit amount is " + MINDEPOSIT.ToString("C"));
            }

            var account = ReadFromBinary(iban);
            account.Deposit(amount);
            WriteToBinary(account);

            return account.Balance;
        }

        /// <summary>
        /// Makes a withdrawal of <paramref name="amount"/> from an account with specified <paramref name="iban"/>
        /// </summary>
        /// <param name="iban">IBAN of an account to make withdrawal</param>
        /// <param name="amount">withdrawal amount</param>
        /// <returns>new account balance</returns>
        /// <exception cref="ArgumentException">withdrawal amount is lesser than minimal</exception>
        public decimal MakeWithdrawal(string iban, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(iban))
            {
                throw new ArgumentException("No IBAN is given.", "IBAN");
            }

            if (amount < MINWITHDRAWAL)
            {
                throw new ArgumentException("Minimum withdrawal amount is " + MINWITHDRAWAL.ToString("C"));
            }

            var account = ReadFromBinary(iban);
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
            return ibanGenerator.GenerateIBAN();
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
        /// <param name="iban">IBAN of an account to remove</param>
        /// <param name="balance">account balance</param>
        /// <returns>account balance</returns>
        decimal IAccountFileService.RemoveAccount(string iban)
        {
            return RemoveFromBinary(iban);
        }

        /// <summary>
        /// Gets an account from a file
        /// </summary>
        /// <param name="iban">IBAN of an account</param>
        /// <returns>bank account instance</returns>
        BankAccount IAccountFileService.ReadAccount(string iban)
        {
            return ReadFromBinary(iban);
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
                    writer.Write(account.Owner);
                    writer.Write(account.Balance);
                    writer.Write(account.BonusPoints);
                    writer.Write(account.GetType().FullName);
                }
            }
        }

        /// <summary>
        /// removes an account from binary file
        /// </summary>
        /// <param name="iban">IBAN of an account to remove</param>
        /// <returns>account balance</returns>
        private decimal RemoveFromBinary(string iban)
        {
            using (var stream = new FileStream(accountsFilePath, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    while (reader.ReadString() != iban)
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
                    var offset = (iban.Length + 1) + (owner.Length + 1) + sizeof(decimal) + sizeof(float) + (type.Length + 1);
                    byte[] array = new byte[stream.Length - stream.Position];
                    stream.Read(array, 0, array.Length);
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
        /// <param name="iban">IBAN to find</param>
        /// <returns>bank account instance</returns>
        private BankAccount ReadFromBinary(string iban)
        {
            BankAccount account;

            using (var stream = new FileStream(accountsFilePath, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    while (reader.ReadString() != iban)
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
                    account = (BankAccount)Type.GetType(type).GetConstructors(BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.NonPublic)[0].Invoke(new object[] { iban, owner, balance, bonus });
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
                        writer.Write(account.Owner);
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
