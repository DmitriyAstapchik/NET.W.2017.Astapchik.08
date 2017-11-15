using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountSystem
{
    public class ListStorage : IAccountStorage
    {
        private List<BankAccount> storage = new List<BankAccount>();

        public void AddAccount(BankAccount account)
        {
            if (storage.Contains(account))
            {
                throw new ApplicationException($"list already contains an account with IBAN {account.IBAN}");
            }

            storage.Add(account);
        }

        public BankAccount GetAccount(string iban)
        {
            return storage.Find(acc => acc.IBAN == iban) ?? throw new ApplicationException($"account with IBAN {iban} was not found");
        }

        public decimal RemoveAccount(string iban)
        {
            var account = GetAccount(iban);
            if (storage.Contains(account))
            {
                if (storage.Remove(account))
                {
                    return account.Balance;
                }

                throw new ApplicationException($"account with IBAN {iban} was not removed");
            }

            throw new ApplicationException($"list does not contain an account with IBAN {iban}");
        }

        public void SaveAccount(BankAccount account)
        {
            BankAccount old;
            try
            {
                old = GetAccount(account.IBAN);
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }
    }
}
