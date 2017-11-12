using System;
using System.Collections.Generic;
using System.Globalization;

namespace BookSystem.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-us");

            var book1 = new Book("978-1533134134", "Amy Harmon", "The Bird and the Sword", "CreateSpace Independent Publishing Platform", DateTime.Parse("05.06.2016"), 336, 11.89m);
            var book2 = new Book("978-1542046633", "John Marrs", "The Good Samaritan", "Thomas & Mercer", new DateTime(2017, 12, 1), 400, 10.97m);
            var book3 = new Book("978-1516837052", "John Marrs", "Welcome To Wherever You Are", "CreateSpace Independent Publishing Platform", new DateTime(2015, 09, 11), 366, 12.49m);

            var books = new SortedSet<Book>
            {
                book1,
                null,
                book2,
                book3
            };

            Console.WriteLine("- sorted set order is alphabetical:");
            foreach (var book in books)
            {
                Console.WriteLine(book);
            }

            var book2Fake = new Book(book2.ISBN, "asd", "asd", "asd", DateTime.Today, default(ushort), default(decimal));
            if (Equals(book2, book2Fake) && book2.GetHashCode() == book2Fake.GetHashCode())
            {
                Console.WriteLine("\n- different book instances with same ISBN are equal and have equal hash codes");
            }

            var service1 = new BookListService(books);
            Console.WriteLine("\n- book list service #1 is created based on a sorted set");

            var book4 = new Book("978-1335005106", "John Marrs", "The One", "Hanover Square", new DateTime(2018, 02, 20), 368, 26.99m);
            service1.AddBook(book4);
            try
            {
                Console.WriteLine("\n- trying to add the same book one more time:");
                service1.AddBook(book4);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            service1.RemoveBook(book4);
            try
            {
                Console.WriteLine("\n- trying to remove the same book one more time:");
                service1.RemoveBook(book4);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("\n- find a book with author Amy Harmon:");
            Console.WriteLine(service1.FindBookByTag(new AuthorCriteria()));

            Console.WriteLine("\n- find a book with price less than 11:");
            Console.WriteLine(service1.FindBookByTag(new PriceCriteria()));

            service1.SortBooksByTag(new DateComparer());
            Console.WriteLine("\n- books order after a date sort:");
            foreach (var book in service1.Books)
            {
                Console.WriteLine(book);
            }

            service1.SortBooksByTag(new PriceComparer());
            Console.WriteLine("\n- books order after a price sort:");
            foreach (var book in service1.Books)
            {
                Console.WriteLine(book);
            }

            service1.SaveToStorage("BookListStorage");
            Console.WriteLine("\n- saved to BookListStorage");

            var service2 = new BookListService("BookListStorage");
            Console.WriteLine("\n- book list service #2 is created based on BookListStorage:");
            foreach (var book in service2.Books)
            {
                Console.WriteLine(book);
            }

            Console.WriteLine("\n- remove a book by Amy Harmon, save list to the storage and load the storage to the service #1:");
            service2.RemoveBook(service2.FindBookByTag(new AuthorCriteria()));
            service2.SaveToStorage("BookListStorage");
            service1.LoadFromStorage("BookListStorage");
            foreach (var book in service1.Books)
            {
                Console.WriteLine(book);
            }

            Console.WriteLine($"\n- book4.ToLongString():\n{book4.ToLongString()}");

            Console.Read();
        }

        class AuthorCriteria : BookListService.IBookCriteria
        {
            public bool IsEligible(Book book)
            {
                return book != null && book.Author == "Amy Harmon";
            }
        }

        class PriceCriteria : BookListService.IBookCriteria
        {
            public bool IsEligible(Book book)
            {
                return book != null && book.Price < 11;
            }
        }

        class DateComparer : IComparer<Book>
        {
            public int Compare(Book book1, Book book2)
            {
                return book1 != null && book2 != null ? book1.PublicationDate.CompareTo(book2.PublicationDate) : book1 == null ? 1 : -1;
            }
        }

        class PriceComparer : IComparer<Book>
        {
            public int Compare(Book book1, Book book2)
            {
                return book1 != null && book2 != null ? book1.Price.CompareTo(book2.Price) : book1 == null ? 1 : -1;
            }
        }
    }
}
