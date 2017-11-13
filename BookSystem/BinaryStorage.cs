using System;
using System.Collections.Generic;
using System.IO;

namespace BookSystem
{
    /// <summary>
    /// class for work with binary storage of books
    /// </summary>
    public class BinaryStorage : IBookListStorage
    {
        /// <summary>
        /// path to binary file
        /// </summary>
        private string path;

        /// <summary>
        /// initializes path to storage file
        /// </summary>
        /// <param name="path">path to binary file</param>
        public BinaryStorage(string path)
        {
            this.path = path;
        }

        /// <summary>
        /// gets a book list from a storage
        /// </summary>
        /// <returns>list of books from a storage</returns>
        public IEnumerable<Book> LoadFromStorage()
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    List<Book> books = new List<Book>();

                    while (reader.PeekChar() != -1)
                    {
                        var isbn = reader.ReadString();
                        var author = reader.ReadString();
                        var title = reader.ReadString();
                        var publisher = reader.ReadString();
                        var ticks = reader.ReadInt64();
                        var pages = reader.ReadUInt16();
                        var price = reader.ReadDecimal();

                        books.Add(new Book(isbn, author, title, publisher, new DateTime(ticks), pages, price));
                    }

                    return books;
                }
            }
        }

        /// <summary>
        /// writes book list to the storage
        /// </summary>
        /// <param name="books">list of books</param>
        public void SaveToStorage(IEnumerable<Book> books)
        {
            using (var stream = new FileStream(path, FileMode.Create))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    foreach (var book in books)
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
    }
}
