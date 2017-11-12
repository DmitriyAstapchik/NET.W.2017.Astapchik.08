using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace BookSystem
{
    /// <summary>
    /// Represents a service for working with a book collection
    /// </summary>
    public class BookListService : IBookStorageService
    {
        /// <summary>
        /// Set of books to work with
        /// </summary>
        private HashSet<Book> bookSet;

        /// <summary>
        /// Constructs a service for working with specified <paramref name="books"/>
        /// </summary>
        /// <param name="books"></param>
        public BookListService(ICollection<Book> books)
        {
            bookSet = new HashSet<Book>(books.Where(book => book != null));
        }

        /// <summary>
        /// Constructs a service for working with a book set from <paramref name="filePath"/>
        /// </summary>
        /// <param name="filePath">file path to a book set storage</param>
        public BookListService(string filePath)
        {
            bookSet = new HashSet<Book>();
            LoadFromStorage(filePath);
        }

        /// <summary>
        /// Get accessor to a book set
        /// </summary>
        public HashSet<Book> Books => bookSet;

        /// <summary>
        /// Adds a <paramref name="book"/> to the book set
        /// </summary>
        /// <param name="book">a book to add</param>
        /// <exception cref="ArgumentException">the book list already contains specified <paramref name="book"/></exception>
        public void AddBook(Book book)
        {
            if (!bookSet.Add(book))
            {
                throw new ArgumentException("The book list already contains a book with given ISBN");
            }
        }

        /// <summary>
        /// Removes a <paramref name="book"/> from the book set
        /// </summary>
        /// <param name="book">a book to remove</param>
        /// <exception cref="ArgumentException">the book list does not contain specified <paramref name="book"/></exception>
        public void RemoveBook(Book book)
        {
            if (!bookSet.Remove(book))
            {
                throw new ArgumentException("The book list does not contain a book with given ISBN");
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

        /// <summary>
        /// Loads a book set from a binary file
        /// </summary>
        /// <param name="path">a path to a binary storage file</param>
        public void LoadFromStorage(string path)
        {
            bookSet.Clear();

            using (var stream = new FileStream(path, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    while (reader.PeekChar() != -1)
                    {
                        var isbn = reader.ReadString();
                        var author = reader.ReadString();
                        var title = reader.ReadString();
                        var publisher = reader.ReadString();
                        var ticks = reader.ReadInt64();
                        var pages = reader.ReadUInt16();
                        var price = reader.ReadDecimal();

                        bookSet.Add(new Book(isbn, author, title, publisher, new DateTime(ticks), pages, price));
                    }
                }
            }
        }

        /// <summary>
        /// Saves a book set to a binary file
        /// </summary>
        /// <param name="path">a path to a binary storage file</param>
        public void SaveToStorage(string path)
        {
            using (var stream = new FileStream(path, FileMode.Create))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    foreach (var book in bookSet)
                    {
                        writer.Write(book.ISBN);
                        writer.Write(book.Author);
                        writer.Write(book.Title);
                        writer.Write(book.Publisher);
                        writer.Write(book.PublicationDate.Ticks);
                        writer.Write(book.Pages);
                        writer.Write(book.Price);
                    }
                }
            }
        }

        /// <summary>
        /// Represents functionality for books to determine meeting some criteria
        /// </summary>
        public interface IBookCriteria
        {
            bool IsEligible(Book book);
        }
    }
}
