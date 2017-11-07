using System;
using System.Globalization;

namespace BookSystem
{
    /// <summary>
    /// Represents a book with its detailed info, capable of having equality and order relations
    /// </summary>
    public class Book : IEquatable<Book>, IComparable<Book>
    {
        /// <summary>
        /// Constructs a book instance with specified ISBN, author, title, publisher, publication date, pages and price
        /// </summary>
        /// <param name="isbn">book's ISBN</param>
        /// <param name="author">book's author</param>
        /// <param name="title">book's title</param>
        /// <param name="publisher">book's publisher</param>
        /// <param name="date">book's publication date</param>
        /// <param name="pages">book's number of pages</param>
        /// <param name="price">book's price</param>
        public Book(string isbn, string author, string title, string publisher, DateTime date, ushort pages, decimal price)
        {
            ISBN = isbn;
            Author = author;
            Title = title;
            Publisher = publisher;
            PublicationDate = date;
            Pages = pages;
            Price = price;
        }

        /// <summary>
        /// International Standard Book Number of a book
        /// </summary>
        public string ISBN { get; private set; }

        /// <summary>
        /// Author of a book
        /// </summary>
        public string Author { get; private set; }

        /// <summary>
        /// Title of a book
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Publisher of a book
        /// </summary>
        public string Publisher { get; private set; }

        /// <summary>
        /// Publication date of a book
        /// </summary>
        public DateTime PublicationDate { get; private set; }

        /// <summary>
        /// Number of pages of a book
        /// </summary>
        public ushort Pages { get; private set; }

        /// <summary>
        /// Price of a book
        /// </summary>
        public decimal Price { get; private set; }

        /// <summary>
        /// Determines whether the specified object is equal to the current object
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Equals((Book)obj);
        }

        /// <summary>
        /// Gets hash code based on ISBN of a book
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ISBN.GetHashCode();
        }

        /// <summary>
        /// Gets general book info string
        /// </summary>
        /// <returns>general book info</returns>
        public override string ToString()
        {
            return $"{Title} ({PublicationDate.Year}) by {Author} for {Price.ToString("C")}";
        }

        public string ToLongString()
        {
            return string.Join(Environment.NewLine, $"ISBN: {ISBN}", $"Author: {Author}", $"Title: {Title}", $"Publisher: {Publisher}", $"Publication date: {PublicationDate.ToShortDateString()}", $"Pages: {Pages}", $"Price: {Price.ToString("C")}");
        }

        /// <summary>
        /// Determines whether the specified book's ISBN is equal to the current book's ISBN
        /// </summary>
        /// <param name="other">The book instance to compare with the current book instance</param>
        /// <returns>true if the specified book's ISBN is equal to the current book's ISBN; otherwise, false</returns>
        public bool Equals(Book other)
        {
            return other != null && (ReferenceEquals(this, other) || ISBN == other.ISBN);
        }

        /// <summary>
        /// Compares the current book instance with another book instance.
        /// </summary>
        /// <param name="other">A book instance to compare with this instance</param>
        /// <returns>A value that indicates the relative order of the books being compared</returns>
        public int CompareTo(Book other)
        {
            return other != null ? (Author + Title).CompareTo(other.Author + other.Title) : -1;
        }
    }
}
