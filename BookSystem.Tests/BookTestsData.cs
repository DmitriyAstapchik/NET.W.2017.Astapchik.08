using System;
using System.Collections;
using NUnit.Framework;

namespace BookSystem.Tests
{
    internal class BookTestsData
    {
        private static IFormatProvider provider = new BookFormatter();

        public static IEnumerable ToStringTestCases
        {
            get
            {
                yield return new TestCaseData("AT", null).Returns("Jeffrey Richter, CLR via C#");
                yield return new TestCaseData("ATBY", null).Returns("Jeffrey Richter, CLR via C#, \"Microsoft Press\", 2012");
                yield return new TestCaseData("IATBYP", null).Returns("ISBN 13: 978-0-7356-6745-7, Jeffrey Richter, CLR via C#, \"Microsoft Press\", 2012, P. 826");
                yield return new TestCaseData("IATBYPC", null).Returns("ISBN 13: 978-0-7356-6745-7, Jeffrey Richter, CLR via C#, \"Microsoft Press\", 2012, P. 826, 59.99$");
                yield return new TestCaseData("t", null).Returns("CLR via C#");
                yield return new TestCaseData("bYaI", null).Returns("\"Microsoft Press\", 2012, Jeffrey Richter, ISBN 13: 978-0-7356-6745-7");
                yield return new TestCaseData("taci", null).Returns("CLR via C#, Jeffrey Richter, 59.99$, ISBN 13: 978-0-7356-6745-7");
                yield return new TestCaseData("ATby", null).Returns("Jeffrey Richter, CLR via C#, \"Microsoft Press\", 2012");
                yield return new TestCaseData(null, null).Returns("CLR via C# (2012) by Jeffrey Richter for $59.99");
                yield return new TestCaseData(string.Empty, null).Returns("CLR via C# (2012) by Jeffrey Richter for $59.99");
                yield return new TestCaseData("G", provider).Returns("CLR via C# (2012) by Jeffrey Richter for $59.99");
                yield return new TestCaseData("g", provider).Returns("CLR via C# (2012) by Jeffrey Richter for $59.99");
                yield return new TestCaseData("L", provider).Returns(string.Join(Environment.NewLine, "ISBN: 978-0-7356-6745-7", "Author: Jeffrey Richter", "Title: CLR via C#", "Publisher: Microsoft Press", "Publication date: 1/1/2012", "Pages: 826", "Price: $59.99"));
                yield return new TestCaseData(null, provider).Returns("CLR via C# (2012) by Jeffrey Richter for $59.99");
            }
        }

        public static IEnumerable ToStringExceptionsTestCases
        {
            get
            {
                yield return new TestCaseData("G", null);
                yield return new TestCaseData("ATL", null);
                yield return new TestCaseData(" ", null);
                yield return new TestCaseData("f", provider);
            }
        }
    }
}
