using System;
using NUnit.Framework;

namespace BookSystem.Tests
{
    [TestFixture, SetCulture("en-us")]
    public class BookTest
    {
        private static Book book = new Book("978-0-7356-6745-7", "Jeffrey Richter", "CLR via C#", "Microsoft Press", DateTime.Parse("2012.1.1"), 826, 59.99m);

        [TestCase("AT", ExpectedResult = "Jeffrey Richter, CLR via C#")]
        [TestCase("ATBY", ExpectedResult = "Jeffrey Richter, CLR via C#, \"Microsoft Press\", 2012")]
        [TestCase("IATBYP", ExpectedResult = "ISBN 13: 978-0-7356-6745-7, Jeffrey Richter, CLR via C#, \"Microsoft Press\", 2012, P. 826")]
        [TestCase("IATBYPC", ExpectedResult = "ISBN 13: 978-0-7356-6745-7, Jeffrey Richter, CLR via C#, \"Microsoft Press\", 2012, P. 826, 59.99$")]
        [TestCase("t", ExpectedResult = "CLR via C#")]
        [TestCase("bYaI", ExpectedResult = "\"Microsoft Press\", 2012, Jeffrey Richter, ISBN 13: 978-0-7356-6745-7")]
        [TestCase("taci", ExpectedResult = "CLR via C#, Jeffrey Richter, 59.99$, ISBN 13: 978-0-7356-6745-7")]
        public string ToStringTest(string format)
        {
            return book.ToString(format);
        }

        [TestCase("")]
        [TestCase("w")]
        [TestCase("ATW")]
        public void ToStringExceptionTest(string format)
        {
            Assert.That(() => book.ToString(format), Throws.Exception);
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
