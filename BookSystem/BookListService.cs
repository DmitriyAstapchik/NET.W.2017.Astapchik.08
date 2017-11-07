using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Globalization;

namespace BookSystem
{
    /// <summary>
    /// Represents a service for working with a book collection
    /// </summary>
    public class BookListService
    {
        /// <summary>
        /// List of books to work with
        /// </summary>
        public readonly List<Book> bookList;

        /// <summary>
        /// Current book list storage path
        /// </summary>
        private string storagePath = "BookListStorage";

        /// <summary>
        /// Constructs a service for working with specified <paramref name="books"/>
        /// </summary>
        /// <param name="books"></param>
        public BookListService(ICollection<Book> books)
        {
            bookList = new List<Book>(books.Where(book => book != null));
        }

        /// <summary>
        /// Constructs a service for working with a book list from <paramref name="files"/>
        /// </summary>
        /// <param name="files">file paths to book list storages</param>
        public BookListService(params string[] files)
        {
            bookList = new List<Book>();
            LoadFromBinary(files);
        }

        /// <summary>
        /// Adds a <paramref name="book"/> to the book list
        /// </summary>
        /// <param name="book">a book to add</param>
        /// <exception cref="ArgumentException">the book list already contains specified <paramref name="book"/></exception>
        public void AddBook(Book book)
        {
            if (bookList.Contains(book))
            {
                throw new ArgumentException("The book list already contains a book with given ISBN");
            }

            bookList.Add(book);
        }

        /// <summary>
        /// Removes a <paramref name="book"/> from the book list
        /// </summary>
        /// <param name="book">a book to remove</param>
        /// <returns>true if item is successfully removed; otherwise, false</returns>
        /// <exception cref="ArgumentException">the book list does not contain specified <paramref name="book"/></exception>
        public bool RemoveBook(Book book)
        {
            if (!bookList.Contains(book))
            {
                throw new ArgumentException("The book list does not contain a book with given ISBN");
            }

            return bookList.Remove(book);
        }

        /// <summary>
        /// Finds a book by specified <paramref name="criteria"/>
        /// </summary>
        /// <param name="criteria">criteria of eligibility</param>
        /// <returns>the first book that matches the criteria, if found; otherwise, null.</returns>
        public Book FindBookByTag(IBookCriteria criteria)
        {
            return bookList.Find(book => criteria.IsEligible(book));
        }

        /// <summary>
        /// Sorts the book list using the specified <paramref name="comparer"/>
        /// </summary>
        /// <param name="comparer">comparer used for sort</param>
        public void SortBooksByTag(IComparer<Book> comparer)
        {
            bookList.Sort(comparer);
        }

        /// <summary>
        /// Saves the book list to a binary file
        /// </summary>
        /// <param name="path">a path to an output file</param>
        public void SaveToBinary(string path)
        {
            using (var stream = new FileStream(path, FileMode.Create))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    storagePath = path;

                    foreach (var book in bookList)
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
        /// Saves the book list to the last used binary file
        /// </summary>
        public void SaveToBinary()
        {
            SaveToBinary(storagePath);
        }

        /// <summary>
        /// Loads the aggregated book list from binary files
        /// </summary>
        /// <param name="paths">file paths to binary files</param>
        public void LoadFromBinary(params string[] paths)
        {
            bookList.Clear();

            foreach (var path in paths)
            {
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    using (var reader = new BinaryReader(stream))
                    {
                        storagePath = path;

                        while (reader.PeekChar() != -1)
                        {
                            var isbn = reader.ReadString();
                            var author = reader.ReadString();
                            var title = reader.ReadString();
                            var publisher = reader.ReadString();
                            var ticks = reader.ReadInt64();
                            var pages = reader.ReadUInt16();
                            var price = reader.ReadDecimal();

                            bookList.Add(new Book(isbn, author, title, publisher, new DateTime(ticks), pages, price));
                        }
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
