using NUnit.Framework;

namespace CsvParserLib.Tests
{
    public class ColumnTypeDateTimeTests
    {
        [TestCase("31.12.2020")]
        [TestCase("2020-12-31")]
        [TestCase("2020/12/31")]
        public void CanBeParsed_CorrectDate_ExpectTrue(string value)
        {
            Assert.True(new ColumnTypeDate().CanBeParsed(value));
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase(" ")]
        [TestCase("31.02.2020")]
        [TestCase("Hoba")]
        public void CanBeParsed_IncorrectString_ExpectFalse(string value)
        {
            Assert.False(new ColumnTypeDate().CanBeParsed(value));
        }

        [TestCase("31.12.2020", "31.12.2020")]
        [TestCase("31.12.2020", "2020-12-31")]
        [TestCase("31.12.2020", "2020/12/31")]
        public void IsEqual_TwoCorrectDates_ExpectTrue(string a, string b)
        {
            Assert.True(new ColumnTypeDate().IsEqual(a, b));
        }

        [Test]
        public void IsEqual_CorrectDateAndIncorrectDate_ExpectFalse()
        {
            Assert.False(new ColumnTypeDate().IsEqual("31.12.2020", "31.02.2020"));
        }

        [TestCase("31.12.2020", "0")]
        [TestCase("31.12.2020", " ")]
        [TestCase("31.12.2020", "")]
        [TestCase("31.12.2020", null)]
        public void IsEqual_CorrectDateAndIncorrectString_ExpectFalse(string a, string b)
        {
            Assert.False(new ColumnTypeDate().IsEqual(a,b));
        }
    }
}