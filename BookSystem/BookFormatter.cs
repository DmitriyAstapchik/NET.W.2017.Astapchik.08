using System;

namespace BookSystem
{
    /// <summary>
    /// Class provides a formatted string representation of a book
    /// </summary>
    public class BookFormatter : ICustomFormatter, IFormatProvider
    {
        /// <summary>
        /// Converts a specified book to an equivalent string representation using specified format
        /// </summary>
        /// <param name="format">format specifier</param>
        /// <param name="arg">formatted object</param>
        /// <param name="formatProvider">format provider</param>
        /// <returns>string representation of a book</returns>
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg is Book book)
            {
                if (format.ToUpper() == "G")
                {
                    return book.ToString();
                }

                if (format.ToUpper() == "L")
                {
                    return book.ToLongString();
                }

                throw new ArgumentException($"Invalid format specifier {format}", nameof(format));
            }

            throw new FormatException("No availiable format for an object.");
        }

        /// <summary>
        /// Returns an object that provides formatting services for a book
        /// </summary>
        /// <param name="formatType">An object that specifies the type of format object to return.</param>
        /// <returns>This instance if <paramref name="formatType"/> is BookFormatter; otherwise, null.</returns>
        public object GetFormat(Type formatType)
        {
            return formatType == typeof(ICustomFormatter) ? this : null;
        }
    }
}
