using System;
using NUnit.Framework;

namespace BookSystem.Tests
{
    [TestFixture, SetCulture("en-us")]
    public class BookTests
    {
        private static Book book = new Book("978-0-7356-6745-7", "Jeffrey Richter", "CLR via C#", "Microsoft Press", DateTime.Parse("2012.1.1"), 826, 59.99m);

        [Test, TestCaseSource(typeof(BookTestsData), "ToStringTestCases")]
        public string ToStringTest(string format, IFormatProvider provider)
        {
            return book.ToString(format, provider);
        }

        [Test, TestCaseSource(typeof(BookTestsData), "ToStringExceptionsTestCases")]
        public void ToStringExceptionsTest(string format, IFormatProvider provider)
        {
            Assert.That(() => book.ToString(format, provider), Throws.Exception);
        }

        [TestCase("{0:atb}", ExpectedResult = "Jeffrey Richter, CLR via C#, \"Microsoft Press\"")]
        [TestCase("{0:ByaI}", ExpectedResult = "\"Microsoft Press\", 2012, Jeffrey Richter, ISBN 13: 978-0-7356-6745-7")]
        [TestCase("{0:TACI}", ExpectedResult = "CLR via C#, Jeffrey Richter, 59.99$, ISBN 13: 978-0-7356-6745-7")]
        public string FormatTest(string format)
        {
            return string.Format(format, book);
        }
    }
}
