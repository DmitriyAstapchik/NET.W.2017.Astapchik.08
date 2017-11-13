using System.Collections.Generic;

namespace BookSystem
{
    /// <summary>
    /// methods for work with a book storage
    /// </summary>
    public interface IBookListStorage
    {
        /// <summary>
        /// loads book list from a storage
        /// </summary>
        /// <returns>enumerable books</returns>
        IEnumerable<Book> LoadFromStorage();

        /// <summary>
        /// saves book list to a storage
        /// </summary>
        /// <param name="books">enumerable books</param>
        void SaveToStorage(IEnumerable<Book> books);
    }
}
