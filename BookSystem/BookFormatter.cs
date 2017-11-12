using System;
using System.Text;

namespace BookSystem
{
    /// <summary>
    /// Class provides a formattable string representation of a book
    /// </summary>
    public abstract class BookFormatter : ICustomFormatter, IFormatProvider
    {
        /// <summary>
        /// Converts a specified book to an equivalent string representation using specified format
        /// </summary>
        /// <param name="format">format specifier</param>
        /// <param name="arg">formattable object</param>
        /// <param name="formatProvider">format provider</param>
        /// <returns>string representation of a book</returns>
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                throw new ArgumentException("Null or empty format string.");
            }

            if (arg is Book book)
            {
                var sb = new StringBuilder(format.Length);

                foreach (var ch in format)
                {
                    switch (char.ToUpper(ch))
                    {
                        case 'I':
                            sb.Append("ISBN 13: " + book.ISBN);
                            break;
                        case 'A':
                            sb.Append(book.Author);
                            break;
                        case 'T':
                            sb.Append(book.Title);
                            break;
                        case 'B':
                            sb.Append('"' + book.Publisher + '"');
                            break;
                        case 'Y':
                            sb.Append(book.PublicationDate.Year);
                            break;
                        case 'P':
                            sb.Append("P. " + book.Pages);
                            break;
                        case 'C':
                            sb.Append(book.Price + "$");
                            break;
                        default:
                            throw new FormatException($"Invalid format identifier {ch}");
                    }
                    sb.Append(", ");
                }
                sb.Remove(sb.Length - 2, 2);

                return sb.ToString();
            }
            else
            {
                throw new FormatException("No availiable format for an object.");
            }
        }

        /// <summary>
        /// Returns an object that provides formatting services for a book
        /// </summary>
        /// <param name="formatType">An object that specifies the type of format object to return.</param>
        /// <returns>This instance if <paramref name="formatType"/> is BookFormatter; otherwise, null.</returns>
        public object GetFormat(Type formatType)
        {
            return formatType == typeof(BookFormatter) ? this : null;
        }
    }
}
