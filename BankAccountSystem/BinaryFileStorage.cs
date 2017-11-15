using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountSystem
{
    /// <summary>
    /// class for work with binary accounts storage file
    /// </summary>
    public class BinaryFileStorage : IAccountStorage
    {
        /// <summary>
        /// path to a binary file
        /// </summary>
        private string path;

        /// <summary>
        /// create an instance to work with file on <paramref name="path"/>
        /// </summary>
        /// <param name="path">path to a binary file</param>
        public BinaryFileStorage(string path)
        {
            this.path = path;
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
        }

        /// <summary>
        /// appends an account info to a file
        /// </summary>
        /// <param name="account">account to add</param>
        public void AddAccount(BankAccount account)
        {
            using (var stream = new FileStream(path, FileMode.Append))
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
        /// gets a bank account from a file
        /// </summary>
        /// <param name="iban">IBAN to find</param>
        /// <returns>account with specified <paramref name="iban"/></returns>
        public BankAccount GetAccount(string iban)
        {
            BankAccount account;

            using (var stream = new FileStream(path, FileMode.Open))
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
        /// removes a bank account from a file
        /// </summary>
        /// <param name="iban">IBAN of an account to remove</param>
        /// <returns>account balance</returns>
        public decimal RemoveAccount(string iban)
        {
            using (var stream = new FileStream(path, FileMode.Open))
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
        /// renew an account info
        /// </summary>
        /// <param name="account">account to renew</param>
        public void SaveAccount(BankAccount account)
        {
            using (var stream = new FileStream(path, FileMode.Open))
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
    }
}
