using System;
using System.Collections.Generic;
using System.Linq;

namespace BookSystem
{
    /// <summary>
    /// Represents a service for working with a book collection
    /// </summary>
    public class BookListService
    {
        /// <summary>
        /// Set of books to work with
        /// </summary>
        private HashSet<Book> bookSet;

        /// <summary>
        /// Constructs a service for working with specified <paramref name="books"/>
        /// </summary>
        /// <param name="books"></param>
        public BookListService(IEnumerable<Book> books)
        {
            bookSet = new HashSet<Book>(books.Where(book => book != null));
        }

        /// <summary>
        /// Constructs a service for working with a book set from <paramref name="storage"/>
        /// </summary>
        /// <param name="storage">a book set storage</param>
        public BookListService(IBookListStorage storage)
        {
            this.Storage = storage;
            bookSet = new HashSet<Book>(storage.LoadFromStorage());
        }

        /// <summary>
        /// Represents functionality for books to determine meeting some criteria
        /// </summary>
        public interface IBookCriteria
        {
            bool IsEligible(Book book);
        }

        /// <summary>
        /// Get accessor to a book set
        /// </summary>
        public HashSet<Book> Books => bookSet;

        /// <summary>
        /// book set storage
        /// </summary>
        public IBookListStorage Storage { get; set; }

        /// <summary>
        /// Adds a <paramref name="book"/> to the book set
        /// </summary>
        /// <param name="book">a book to add</param>
        /// <exception cref="ApplicationException">the book list already contains specified <paramref name="book"/></exception>
        public void AddBook(Book book)
        {
            if (!bookSet.Add(book))
            {
                throw new ApplicationException($"The book list already contains a book with ISBN {book.ISBN}.", new ArgumentException("The book is already present in the hashset."));
            }
        }

        /// <summary>
        /// Removes a <paramref name="book"/> from the book set
        /// </summary>
        /// <param name="book">a book to remove</param>
        /// <exception cref="ApplicationException">the book list does not contain specified <paramref name="book"/></exception>
        public void RemoveBook(Book book)
        {
            if (!bookSet.Remove(book))
            {
                throw new ApplicationException($"The book list does not contain a book with ISBN {book.ISBN}.", new ArgumentException("The book is not found in the hashset"));
            }
        }

        /// <summary>
        /// Finds a book by specified <paramref name="criteria"/>
        /// </summary>
        /// <param name="criteria">criteria of eligibility</param>
        /// <returns>the first book that matches the criteria, if found; otherwise, null.</returns>
        public Book FindBookByTag(IBookCriteria criteria)
        {
            return bookSet.FirstOrDefault(book => criteria.IsEligible(book));
        }

        /// <summary>
        /// Sorts the book list using the specified <paramref name="comparer"/>
        /// </summary>
        /// <param name="comparer">comparer used for sort</param>
        public void SortBooksByTag(IComparer<Book> comparer)
        {
            bookSet = new HashSet<Book>(bookSet.OrderBy(book => book, comparer));
        }
    }
}